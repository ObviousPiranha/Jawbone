using Piranha.Jawbone.Extensions;
using System;

namespace Piranha.Jawbone;

public static class Utf8
{
    private const int LeadMask = (1 << 7) | (1 << 6);
    private const int LeadBit = 1 << 7;
    private const int SixBits = 0x3f;

    private static int CountSigBits(int b)
    {
        int result = 0;

        for (int mask = LeadBit; mask != 0; mask >>= 1)
        {
            if (b.MaskAny(mask))
                ++result;
            else
                break;
        }

        return result;
    }

    public static int Encode(Span<byte> bytes, int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        else if (value < 0x80)
        {
            bytes[0] = (byte)value;
            return 1;
        }
        else if (value < 0x800)
        {
            bytes[1] = (byte)(value & SixBits | LeadBit);
            bytes[0] = (byte)(value >> 6 | 0xc0);
            return 2;
        }
        else if (value < 0x10000)
        {
            bytes[2] = (byte)(value & SixBits | LeadBit);
            bytes[1] = (byte)(value >> 6 & SixBits | LeadBit);
            bytes[0] = (byte)(value >> 12 | 0xe0);
            return 3;
        }
        else if (value < 0x200000)
        {
            bytes[3] = (byte)(value & SixBits | LeadBit);
            bytes[2] = (byte)(value >> 6 & SixBits | LeadBit);
            bytes[1] = (byte)(value >> 12 & SixBits | LeadBit);
            bytes[0] = (byte)(value >> 18 | 0xf0);
            return 4;
        }
        else if (value < 0x4000000)
        {
            bytes[4] = (byte)(value & SixBits | LeadBit);
            bytes[3] = (byte)(value >> 6 & SixBits | LeadBit);
            bytes[2] = (byte)(value >> 12 & SixBits | LeadBit);
            bytes[1] = (byte)(value >> 18 & SixBits | LeadBit);
            bytes[0] = (byte)(value >> 24 | 0xf8);
            return 5;
        }
        else
        {
            bytes[5] = (byte)(value & SixBits | LeadBit);
            bytes[4] = (byte)(value >> 6 & SixBits | LeadBit);
            bytes[3] = (byte)(value >> 12 & SixBits | LeadBit);
            bytes[2] = (byte)(value >> 18 & SixBits | LeadBit);
            bytes[1] = (byte)(value >> 24 & SixBits | LeadBit);
            bytes[0] = (byte)(value >> 30 | 0xfc);
            return 6;
        }
    }

    public static (int codePoint, int length) ReadCodePoint(ReadOnlySpan<byte> utf8)
    {
        var sigBitCount = CountSigBits(utf8[0]);

        if (sigBitCount == 0)
            return (utf8[0], 1);

        if (sigBitCount == 1)
            throw new FormatException("Continuation byte found instead of lead byte");

        if (4 < sigBitCount)
            throw new FormatException("UTF-8 only supports encodings up to 4 bytes.");

        var codePoint = ~(int.MinValue >> (24 + sigBitCount)) & utf8[0];

        for (int i = 1; i < sigBitCount; ++i)
        {
            if ((utf8[i] & LeadMask) != LeadBit)
                throw new FormatException("Missing continuation byte");

            codePoint = (codePoint << 6) | (utf8[i] & SixBits);
        }

        return (codePoint, sigBitCount);
    }

    internal static int GetContinuationByte(int codePoint, int shift) => LeadBit | ((codePoint >> (6 * shift)) & SixBits);
}
