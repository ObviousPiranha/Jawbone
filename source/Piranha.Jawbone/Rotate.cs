using System;

namespace Piranha.Jawbone;

public static class Rotate
{
    public static void Right<T>(Span<T> span, int count)
    {
        if (span.IsEmpty)
            return;
        var n = count % span.Length;
        if (n == 0)
            return;
        if (n < 0)
            n += span.Length;
        span.Reverse();
        span[..n].Reverse();
        span[n..].Reverse();
    }

    public static void Left<T>(Span<T> span, int count) => Right(span, -count);
}
