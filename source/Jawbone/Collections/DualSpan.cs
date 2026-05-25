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

    public ref T this[int index]
    {
        get
        {
            if (index < First.Length)
                return ref First[index];
            else
                return ref Second[index - First.Length];
        }
    }

    public ref T this[Index index] => ref this[index.GetOffset(Length)];

    public DualSpan(Span<T> first) => First = first;

    public DualSpan(
        Span<T> first,
        Span<T> second)
    {
        First = first;
        Second = second;
    }

    public void Clear()
    {
        First.Clear();
        Second.Clear();
    }

    public void Fill(T value)
    {
        First.Fill(value);
        Second.Fill(value);
    }

    public void Reverse()
    {
        if (TryGetSpan(out var span))
        {
            span.Reverse();
            return;
        }

        var ii = Length;
        var n = ii / 2;
        for (int i = 0; i < n; ++i)
        {
            var swapValue = this[i];
            this[i] = this[--ii];
            this[ii] = swapValue;
        }
    }
    
    public void CopyTo(Span<T> destination)
    {
        Second.CopyTo(destination[First.Length..]);
        First.CopyTo(destination);
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

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator
    {
        private readonly DualSpan<T> _dualSpan;
        private int _index;

        public Enumerator(DualSpan<T> dualSpan)
        {
            _dualSpan = dualSpan;
            _index = -1;
        }

        public readonly ref T Current => ref _dualSpan[_index];
        public bool MoveNext()
        {
            var next = _index + 1;
            if (_dualSpan.Length <= next)
                return false;
            _index = next;
            return true;
        }
    }
}