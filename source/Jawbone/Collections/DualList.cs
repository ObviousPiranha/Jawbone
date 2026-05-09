using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Jawbone;

public sealed class DualList<TLeft, TRight>
{
    private TLeft[] _left = [];
    private TRight[] _right = [];
    private int _count;

    public int Count
    {
        get => _count;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            if (_count < value)
            {
                var index = _count;
                GrowTo(value);
                Left[index..].Clear();
                Right[index..].Clear();
            }
            else
            {
                ShrinkTo(value);
            }
        }
    }
    public int Capacity => _left.Length;
    public Span<TLeft> Left => _left.AsSpan(0, _count);
    public Span<TRight> Right => _right.AsSpan(0, _count);

    public DualValue<TLeft, TRight> this[int index]
    {
        get => new(Left[index], Right[index]);
        set
        {
            Left[index] = value.Left;
            Right[index] = value.Right;
        }
    }

    public DualValue<TLeft, TRight> this[Index index]
    {
        get => this[index.GetOffset(_count)];
        set => this[index.GetOffset(_count)] = value;
    }

    public DualList()
    {
    }

    public DualList(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (capacity == 0)
            return;
        _left = new TLeft[capacity];
        _right = new TRight[capacity];
    }

    public void Add(TLeft left, TRight right)
    {
        var index = _count;
        GrowFor(1);
        _left[index] = left;
        _right[index] = right;
    }

    public void AddSpans(ReadOnlySpan<TLeft> left, ReadOnlySpan<TRight> right)
    {
        if (left.Length != right.Length)
            throw new ArgumentException("Lengths do not match.");
        var index = _count;
        GrowFor(left.Length);
        left.CopyTo(Left[index..]);
        right.CopyTo(Right[index..]);
    }

    public void AddEnumerables(IEnumerable<TLeft> left, IEnumerable<TRight> right)
    {
        if (SpanReader.TryGetSpan(left, out var leftSpan) &&
            SpanReader.TryGetSpan(right, out var rightSpan))
        {
            AddSpans(leftSpan, rightSpan);
            return;
        }

        using var leftEnumerator = left.GetEnumerator();
        using var rightEnumerator = right.GetEnumerator();
        while (true)
        {
            var leftMoved = leftEnumerator.MoveNext();
            var rightMoved = rightEnumerator.MoveNext();
            if (leftMoved != rightMoved)
                throw new ArgumentException("Enumerables did not stop at the same time.");
            if (!leftMoved)
                break;
            Add(leftEnumerator.Current, rightEnumerator.Current);
        }
    }

    public void AddPairs(params ReadOnlySpan<DualValue<TLeft, TRight>> pairs)
    {
        var index = _count;
        GrowFor(pairs.Length);
        var left = Left;
        var right = Right;
        foreach (var pair in pairs)
        {
            left[index] = pair.Left;
            right[index++] = pair.Right;
        }
    }

    public void Clear() => ShrinkTo(0);

    private void ShrinkTo(int nextCount)
    {
        Debug.Assert(nextCount <= _count);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<TLeft>())
            Left[nextCount.._count].Clear();
        if (RuntimeHelpers.IsReferenceOrContainsReferences<TRight>())
            Right[nextCount.._count].Clear();
        _count = nextCount;
    }
    private void GrowFor(int count) => GrowTo(_count + count);
    private void GrowTo(int nextCount)
    {
        if (Capacity < nextCount)
        {
            var nextCapacity = int.Max(Capacity * 2, 8);
            while (nextCapacity < nextCount)
                nextCapacity *= 2;
            Array.Resize(ref _left, nextCapacity);
            Array.Resize(ref _right, nextCapacity);
        }
        _count = nextCount;
    }
}