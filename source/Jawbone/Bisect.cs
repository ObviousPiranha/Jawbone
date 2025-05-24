using System;

namespace Jawbone;

public static class Bisect
{
    public static int Left(ReadOnlySpan<int> values, int value)
    {
        var lo = 0;
        var hi = values.Length;

        while (lo < hi)
        {
            var index = (hi - lo) / 2 + lo;
            if (value <= values[index])
                hi = index;
            else
                lo = index + 1;
        }

        return lo;
    }

    public static int Right(ReadOnlySpan<int> values, int value)
    {
        var lo = 0;
        var hi = values.Length;

        while (lo < hi)
        {
            var index = (hi - lo) / 2 + lo;
            if (value < values[index])
                hi = index;
            else
                lo = index + 1;
        }

        return lo;
    }
}
