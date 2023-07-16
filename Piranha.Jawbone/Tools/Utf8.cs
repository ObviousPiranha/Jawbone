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

        int first;
        int length;

        if (value < 0x800)
        {
            first = 0xc0;
            length = 2;
        }
        else if (value < 0x10000)
        {
            first = 0xe0;
            length = 3;
        }
        else if (value < 0x200000)
        {
            first = 0xf0;
            length = 4;
        }
        else if (value < 0x4000000)
        {
            first = 0xf8;
            length = 5;
        }
        else
        {
            first = 0xfc;
            length = 6;
        }

        bytes[0] = (byte)(first | (value >> (6 * (length - 1))));

        for (int i = 1; i < length; ++i)
        {
            int v = value >> (6 * (length - 1 - i));
            bytes[i] = (byte)(LeadBit | (v & SixBits));
        }

        return length;
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

        for (int i = 1; i <= sigBitCount; ++i)
        {
            if ((utf8[i] & LeadMask) != LeadBit)
                throw new FormatException("Missing continuation byte");

            codePoint = (codePoint << 6) | (utf8[i] & SixBits);
        }

        return (codePoint, sigBitCount);
    }

    internal static int GetContinuationByte(int codePoint, int shift) => LeadBit | ((codePoint >> (6 * shift)) & SixBits);
}
