using System;
using System.Collections.Generic;

namespace Jawbone;

public readonly ref struct DualReadOnlySpan<T>
{
    public ReadOnlySpan<T> First { get; }
    public ReadOnlySpan<T> Second { get; }
    public int Length => First.Length + Second.Length;
    public bool IsEmpty => First.IsEmpty && Second.IsEmpty;
    public bool IsContiguous => Second.IsEmpty || First.IsEmpty;

    public ref readonly T this[int index]
    {
        get
        {
            if (index < First.Length)
                return ref First[index];
            else
                return ref Second[index - First.Length];
        }
    }

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
        Second.CopyTo(destination[First.Length..]);
        First.CopyTo(destination);
    }

    public void CopyTo(DualSpan<T> destination)
    {
        if (destination.First.Length < Length)
        {
            Slice(destination.First.Length).CopyTo(destination.Second);
            Slice(0, destination.First.Length).CopyTo(destination.First);
        }
        else
        {
            CopyTo(destination.First);
        }
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

    public bool SequenceEqual(
        ReadOnlySpan<T> span,
        IEqualityComparer<T>? comparer = null)
    {
        return
            span.Length == Length &&
            First.SequenceEqual(span[..First.Length], comparer) &&
            Second.SequenceEqual(span[First.Length..], comparer);
    }

    public bool TryGetSpan(out ReadOnlySpan<T> result)
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

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator
    {
        private readonly DualReadOnlySpan<T> _dualSpan;
        private int _index;

        public Enumerator(DualReadOnlySpan<T> dualSpan)
        {
            _dualSpan = dualSpan;
            _index = -1;
        }

        public readonly ref readonly T Current => ref _dualSpan[_index];
        public bool MoveNext()
        {
            var next = _index + 1;
            if (_dualSpan.Length <= next)
                return false;
            _index = next;
            return true;
        }
    }

    public static implicit operator DualReadOnlySpan<T>(DualSpan<T> dualSpan) => new(dualSpan);
}

public static class DualReadOnlySpan
{
    public static DualReadOnlySpan<T> AsReadOnly<T>(this DualSpan<T> dualSpan) => dualSpan;
}