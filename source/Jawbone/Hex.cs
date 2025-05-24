using System;

namespace Jawbone;

public static class Hex
{
    public const int InvalidDigit = -1;
    internal const string Lower = "0123456789abcdef";

    public static int MaybeParseDigit(int d)
    {
        if ('0' <= d && d <= '9')
            return d - '0';
        else if ('a' <= d && d <= 'f')
            return d - 'a' + 10;
        else if ('A' <= d && d <= 'F')
            return d - 'A' + 10;
        else
            return InvalidDigit;
    }

    public static int ParseDigit(int d)
    {
        var result = MaybeParseDigit(d);

        if (result == InvalidDigit)
            Throw();

        return result;

        static void Throw() => throw new FormatException("Invalid hex digit");
    }

    public static int MaybeParseDigits(int high, int low)
    {
        var a = MaybeParseDigit(high);

        if (a == InvalidDigit)
            return InvalidDigit;

        var b = MaybeParseDigit(low);

        if (b == InvalidDigit)
            return InvalidDigit;

        return (a << 4) | b;
    }

    public static int ParseDigits(int high, int low) => (ParseDigit(high) << 4) | ParseDigit(low);
}
