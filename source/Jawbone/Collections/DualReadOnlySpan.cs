using System;

namespace Jawbone;

public readonly ref struct DualReadOnlySpan<T>
{
    public ReadOnlySpan<T> First { get; }
    public ReadOnlySpan<T> Second { get; }
    public int Length => First.Length + Second.Length;

    public DualReadOnlySpan(ReadOnlySpan<T> first) => First = first;

    public DualReadOnlySpan(
        ReadOnlySpan<T> first,
        ReadOnlySpan<T> second)
    {
        First = first;
        Second = second;
    }

    public DualReadOnlySpan(DualSpan<T> dualSpan)
    {
        First = dualSpan.First;
        Second = dualSpan.Second;
    }
    
    public void CopyTo(Span<T> destination)
    {
        var span = destination[..Length];
        First.CopyTo(span);
        Second.CopyTo(span[First.Length..]);
    }

    public DualReadOnlySpan<T> Slice(int start)
    {
        if (First.Length <= start)
            return new(Second.Slice(start - First.Length));
        return new(First.Slice(start), Second);
    }

    public DualReadOnlySpan<T> Slice(int start, int length)
    {
        if (First.Length <= start)
            return new(Second.Slice(start - First.Length, length));
        var end = start + length;
        if (end <= First.Length)
            return new(First.Slice(start, length));
        return new(
            First.Slice(start),
            Second.Slice(0, length - First.Length + start));
    }

    public DualReadOnlySpan<T> Slice(Range range)
    {
        var (start, length) = range.GetOffsetAndLength(Length);
        return Slice(start, length);
    }

    public static implicit operator DualReadOnlySpan<T>(DualSpan<T> dualSpan) => new(dualSpan);
}