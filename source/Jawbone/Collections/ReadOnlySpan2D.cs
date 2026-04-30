using System;

namespace Jawbone;

public readonly ref struct ReadOnlySpan2D<T>
{
    public ReadOnlySpan<T> Source { get; }
    public int Width { get; }
    public int Height { get; }
    public int Area => Source.Length;

    public ref readonly T this[int x, int y] => ref GetRow(y)[x];
    public ref readonly T this[Point32 p] => ref GetRow(p.Y)[p.X];

    public ReadOnlySpan2D(ReadOnlySpan<T> source, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 1);
        Source = source.Slice(0, width * height);
        Width = width;
        Height = height;
    }

    public ReadOnlySpan2D(Span2D<T> span)
    {
        Source = span.Source;
        Width = span.Width;
        Height = span.Height;
    }
    
    public ReadOnlySpan<T> GetRow(int y) => Source.Slice(y * Width, Width);
}