using System;

namespace Jawbone;

public readonly ref struct Span2D<T>
{
    public Span<T> Source { get; }
    public int Width { get; }
    public int Height { get; }
    public int Area => Source.Length;

    public ref T this[int x, int y] => ref GetRow(y)[x];
    public ref T this[Point32 p] => ref GetRow(p.Y)[p.X];

    public Span2D(Span<T> source, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 1);
        Source = source.Slice(0, width * height);
        Width = width;
        Height = height;
    }

    public Span<T> GetRow(int y) => Source.Slice(y * Width, Width);

    public static implicit operator ReadOnlySpan2D<T>(Span2D<T> span) => new(span);
}