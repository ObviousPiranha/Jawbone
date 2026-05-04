using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone;

public sealed class UnmanagedDualList<TLeft, TRight>
    where TLeft : unmanaged
    where TRight : unmanaged
{
    private byte[] _bytes = [];
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
                _count = value;
            }
        }
    }

    public int Capacity { get; private set; }
    public Span<byte> LeftBytes => _bytes.AsSpan(0, _count * Unsafe.SizeOf<TLeft>());
    public Span<TLeft> Left => MemoryMarshal.Cast<byte, TLeft>(LeftBytes);
    public Span<byte> RightBytes => _bytes.AsSpan(Capacity * Unsafe.SizeOf<TLeft>(), _count * Unsafe.SizeOf<TRight>());
    public Span<TRight> Right => MemoryMarshal.Cast<byte, TRight>(RightBytes);

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

    public UnmanagedDualList()
    {
    }

    public UnmanagedDualList(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        GrowFor(capacity);
    }

    public void Add(TLeft left, TRight right)
    {
        var index = _count;
        GrowFor(1);
        Left[index] = left;
        Right[index] = right;
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
        if (DualValue.TryGetSpan(left, out var leftSpan) &&
            DualValue.TryGetSpan(right, out var rightSpan))
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

    public void Clear() => _count = 0;

    private void GrowFor(int count) => GrowTo(_count + count);
    private void GrowTo(int nextCount)
    {
        if (Capacity < nextCount)
        {
            var nextCapacity = int.Max(16, Capacity * 2);
            while (nextCapacity < nextCount)
                nextCapacity *= 2;
            var nextBytes = GC.AllocateUninitializedArray<byte>(nextCapacity * BytesPerPair);
            _bytes.AsSpan(0, _count * Unsafe.SizeOf<TLeft>()).CopyTo(nextBytes);
            _bytes.AsSpan(Capacity * Unsafe.SizeOf<TLeft>(), _count * Unsafe.SizeOf<TRight>())
                .CopyTo(nextBytes.AsSpan(nextCapacity * Unsafe.SizeOf<TLeft>()));
            _bytes = nextBytes;
            Capacity = nextCapacity;
        }
        _count = nextCount;
    }

    private static int BytesPerPair => Unsafe.SizeOf<TLeft>() + Unsafe.SizeOf<TRight>();
}