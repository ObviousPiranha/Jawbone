using System;

namespace Jawbone;

public sealed class DualList<T0, T1>
{
    private T0[] _array0 = [];
    private T1[] _array1 = [];

    public int Count { get; private set; }
    public int Capacity => _array0.Length;
    public Span<T0> Left => _array0.AsSpan(0, Count);
    public Span<T1> Right => _array1.AsSpan(0, Count);

    public (T0 item0, T1 item1) this[int index]
    {
        get => (Left[index], Right[index]);
        set
        {
            Left[index] = value.item0;
            Right[index] = value.item1;
        }
    }

    public DualList(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (capacity == 0)
            return;
        _array0 = new T0[capacity];
        _array1 = new T1[capacity];
    }

    public void Add(T0 item0, T1 item1)
    {
        EnsureCapacityFor(1);
        _array0[Count] = item0;
        _array1[Count] = item1;
        ++Count;
    }

    public void Clear()
    {
        Count = 0;
    }

    private void EnsureCapacityFor(int count)
    {
        var minCapacity = Count + count;
        if (minCapacity <= Capacity)
            return;
        var nextCapacity = int.Max(Capacity * 2, 8);
        while (nextCapacity < minCapacity)
            nextCapacity *= 2;
        Array.Resize(ref _array0, nextCapacity);
        Array.Resize(ref _array1, nextCapacity);
    }
}