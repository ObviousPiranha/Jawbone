using System;
using System.Collections.Generic;

namespace Jawbone;

public readonly ref struct DualSpan<T>
{
    public Span<T> First { get; }
    public Span<T> Second { get; }
    public int Length => First.Length + Second.Length;
    public bool IsEmpty => First.IsEmpty && Second.IsEmpty;
    public bool IsContiguous => Second.IsEmpty || First.IsEmpty;

    public DualSpan(Span<T> first) => First = first;

    public DualSpan(
        Span<T> first,
        Span<T> second)
    {
        First = first;
        Second = second;
    }
    
    public void CopyTo(Span<T> destination)
    {
        var span = destination[..Length];
        First.CopyTo(span);
        Second.CopyTo(span[First.Length..]);
    }

    public DualSpan<T> Slice(int start)
    {
        if (First.Length <= start)
            return new(Second.Slice(start - First.Length));
        return new(First.Slice(start), Second);
    }

    public DualSpan<T> Slice(int start, int length)
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

    public DualSpan<T> Slice(Range range)
    {
        var (start, length) = range.GetOffsetAndLength(Length);
        return Slice(start, length);
    }

    public bool SequenceEqual(
        ReadOnlySpan<T> span,
        IEqualityComparer<T>? comparer = null)
    {
        return
            span.Length == Length &&
            First.SequenceEqual(span[..First.Length], comparer) &&
            Second.SequenceEqual(span[First.Length..], comparer);
    }

    public bool TryGetSpan(out Span<T> result)
    {
        if (Second.IsEmpty)
        {
            result = First;
            return true;
        }
        else if (First.IsEmpty)
        {
            result = Second;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }
}