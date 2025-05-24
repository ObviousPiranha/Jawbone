using Jawbone.Extensions;
using System;

namespace Jawbone;

public static class Utf8
{
    private const int LeadMask = 0xc0;
    private const int LeadBit = 0x80;
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

    public static int Encode(this Utf8Span span, int utf32)
    {
        var bytes = span.Bytes;
        if (utf32 < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(utf32));
        }
        else if (utf32 < 0x80)
        {
            bytes[0] = (byte)utf32;
            return 1;
        }
        else if (utf32 < 0x800)
        {
            bytes[1] = GetByte(utf32);
            bytes[0] = (byte)(utf32 >> 6 | 0xc0);
            return 2;
        }
        else if (utf32 < 0x10000)
        {
            bytes[2] = GetByte(utf32);
            bytes[1] = GetByte(utf32 >> 6);
            bytes[0] = (byte)(utf32 >> 12 | 0xe0);
            return 3;
        }
        else if (utf32 < 0x200000)
        {
            bytes[3] = GetByte(utf32);
            bytes[2] = GetByte(utf32 >> 6);
            bytes[1] = GetByte(utf32 >> 12);
            bytes[0] = (byte)(utf32 >> 18 | 0xf0);
            return 4;
        }
        else if (utf32 < 0x4000000)
        {
            bytes[4] = GetByte(utf32);
            bytes[3] = GetByte(utf32 >> 6);
            bytes[2] = GetByte(utf32 >> 12);
            bytes[1] = GetByte(utf32 >> 18);
            bytes[0] = (byte)(utf32 >> 24 | 0xf8);
            return 5;
        }
        else
        {
            bytes[5] = GetByte(utf32);
            bytes[4] = GetByte(utf32 >> 6);
            bytes[3] = GetByte(utf32 >> 12);
            bytes[2] = GetByte(utf32 >> 18);
            bytes[1] = GetByte(utf32 >> 24);
            bytes[0] = (byte)(utf32 >> 30 | 0xfc);
            return 6;
        }

        static byte GetByte(int n) => (byte)(n & SixBits | LeadBit);
    }

    public static (int codePoint, int length) ReadCodePoint(this ReadOnlyUtf8Span utf8)
    {
        var sigBitCount = CountSigBits(utf8.Bytes[0]);

        if (sigBitCount == 0)
            return (utf8.Bytes[0], 1);

        if (sigBitCount == 1)
            throw new FormatException("Continuation byte found instead of lead byte");

        if (4 < sigBitCount)
            throw new FormatException("UTF-8 only supports encodings up to 4 bytes.");

        var codePoint = ~(int.MinValue >> (24 + sigBitCount)) & utf8.Bytes[0];

        for (int i = 1; i < sigBitCount; ++i)
        {
            if ((utf8.Bytes[i] & LeadMask) != LeadBit)
                throw new FormatException("Missing continuation byte");

            codePoint = (codePoint << 6) | (utf8.Bytes[i] & SixBits);
        }

        return (codePoint, sigBitCount);
    }

    internal static int GetContinuationByte(int codePoint, int shift) => LeadBit | ((codePoint >> (6 * shift)) & SixBits);

    public static byte GetHighHexDigit(byte b) => (byte)Hex.Lower[b >> 4];
    public static byte GetLowHexDigit(byte b) => (byte)Hex.Lower[b & 0xf];
}
