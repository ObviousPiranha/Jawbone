using Piranha.Jawbone.Extensions;
using System;

namespace Piranha.Jawbone;

public static class Utf8
{
    private const int LeadMask = (1 << 7) | (1 << 6);
    private const int LeadBit = (1 << 7);
    private const int SixBits = ~(int.MinValue >> 25);

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
