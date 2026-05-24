using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Jawbone;

public sealed class LoopyList<T>
{
    public const int MaxCapacity = 1 << 30;

    private T[] _data = [];
    private int _begin;

    public int Count { get; private set; }
    public int Capacity => _data.Length;
    public int Free => Capacity - Count;
    public bool IsEmpty => Count == 0;
    private int Mask => Capacity - 1;

    private int GetIndex(int offset) => (_begin + offset) & Mask;

    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
            var privateIndex = GetIndex(index);
            var result = _data[privateIndex];
            return result;
        }

        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
            var privateIndex = GetIndex(index);
            _data[privateIndex] = value;
        }
    }

    public T this[Index index]
    {
        get => this[index.GetOffset(Count)];
        set => this[index.GetOffset(Count)] = value;
    }

    public DualSpan<T> this[Range range] => AsSpan(range);

    public bool IsContiguous => (_begin + Count) <= Capacity;

    public LoopyList()
    {
    }

    public LoopyList(int minCapacity)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minCapacity, MaxCapacity);
        GrowTo(minCapacity);
    }

    public DualSpan<T> AsSpan() => GetSpan(0, Count);

    public DualSpan<T> AsSpan(Range range)
    {
        var (start, count) = range.GetOffsetAndLength(Count);
        return AsSpan(start, count);
    }

    public DualSpan<T> AsSpan(int start)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, Count);
        return GetSpan(start, Count - start);
    }

    public DualSpan<T> AsSpan(int start, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, Count);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start + count, Count);
        return GetSpan(start, count);
    }

    private DualSpan<T> GetSpan(int start, int count)
    {
        if (count == 0)
            return default;
        var begin = GetIndex(start);
        var end = GetIndex(start + count);
        if (end <= begin)
        {
            var result = new DualSpan<T>(
                _data.AsSpan(begin),
                _data.AsSpan(0, end));
            return result;
        }
        else
        {
            var result = new DualSpan<T>(
                _data.AsSpan(begin, count));
            return result;
        }
    }

    public void Clear()
    {
        _begin = 0;
        Count = 0;
    }

    public void PushBack(T item)
    {
        EnsureCapacityFor(1);
        var privateIndex = GetIndex(Count++);
        _data[privateIndex] = item;
    }

    public void PushBack(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacityFor(items.Length);
        var end = GetIndex(Count);
        var backCapacity = Capacity - end;
        if (items.Length <= backCapacity)
        {
            items.CopyTo(_data.AsSpan(end));
        }
        else
        {
            items[..backCapacity].CopyTo(_data.AsSpan(end));
            items[backCapacity..].CopyTo(_data);
        }
        Count += items.Length;
    }

    public void PushFront(T item)
    {
        EnsureCapacityFor(1);
        var privateIndex = GetIndex(-1);
        _data[privateIndex] = item;
        _begin = privateIndex;
        ++Count;
    }

    public void PushFront(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacityFor(items.Length);
        if (items.Length <= _begin)
        {
            _begin -= items.Length;
            items.CopyTo(_data.AsSpan(_begin));
        }
        else
        {
            var n = items.Length - _begin;
            _begin += Capacity - items.Length;
            items[n..].CopyTo(_data);
            items[..n].CopyTo(_data.AsSpan(_begin));
        }
        Count += items.Length;
    }

    public T PopBack()
    {
        ThrowIfEmpty();
        var last = GetIndex(Count - 1);
        var result = _data[last];
        if (--Count == 0)
            _begin = 0;
        return result;
    }

    public void PopBackWhile(Predicate<T> predicate) => PopBackWhile(predicate, static (item, state) => state.Invoke(item));
    public void PopBackWhile<TState>(TState arg, Func<T, TState, bool> predicate)
    {
        if (Count == 0)
            return;
        var last = GetIndex(Count - 1);
        while (0 < Count && predicate.Invoke(_data[last], arg))
        {
            last = (last - 1) & Mask;
            --Count;
        }

        if (Count == 0)
            _begin = 0;
    }

    public bool TryPopFront([MaybeNullWhen(false)] out T item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        item = _data[_begin];
        _begin = --Count == 0 ? 0 : GetIndex(1);
        return true;
    }

    public T PopFront()
    {
        ThrowIfEmpty();
        var result = _data[_begin];
        _begin = --Count == 0 ? 0 : GetIndex(1);
        return result;
    }

    public void PopFrontWhile(Predicate<T> predicate) => PopFrontWhile(predicate, static (item, state) => state.Invoke(item));
    public void PopFrontWhile<TState>(TState arg, Func<T, TState, bool> predicate)
    {
        while (0 < Count && predicate.Invoke(_data[_begin], arg))
        {
            _begin = GetIndex(1);
            --Count;
        }

        if (Count == 0)
            _begin = 0;
    }

    public Span<T> AsContiguousSpan()
    {
        if (!IsContiguous)
        {
            var span = _data.AsSpan();
            var n = _data.Length - _begin;
            span.Reverse();
            span[..n].Reverse();
            span[n..].Reverse();
            _begin = 0;
        }

        Debug.Assert(IsContiguous);
        return _data.AsSpan(_begin, Count);
    }

    public void RemoveFront(int count)
    {
        if (count == 0)
            return;
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, Count);
        if (count == Count)
        {
            Clear();
        }
        else
        {
            _begin = GetIndex(count);
            Count -= count;
        }
    }

    public void RemoveBack(int count)
    {
        if (count == 0)
            return;
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, Count);
        if (count == Count)
            Clear();
        else
            Count -= count;
    }

    public void Expand() => GrowTo(0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacityFor(int count)
    {
        var freeCapacity = Capacity - Count;
        if (freeCapacity < count)
            GrowFor(count);
    }

    private void GrowFor(int count)
    {
        var maxFreeCapacity = MaxCapacity - Count;
        if (maxFreeCapacity < count)
            throw new InvalidOperationException("Not enough room for this operation.");
        GrowTo(Count + count);
    }

    private void GrowTo(int minCapacity)
    {
        var nextCapacity = 0 < Capacity ? Capacity * 2 : 16;
        while (nextCapacity < minCapacity)
            nextCapacity *= 2;
        Debug.Assert((nextCapacity & (nextCapacity - 1)) == 0);
        var data = new T[nextCapacity];
        AsSpan().CopyTo(data);
        _data = data;
        _begin = 0;
    }

    private void ThrowIfEmpty()
    {
        if (Count < 1)
            Throw();
        
        [DoesNotReturn] static void Throw() =>
            throw new InvalidOperationException("Collection is empty.");
    }

    public IEnumerable<T> AsEnumerable()
    {
        for (int i = 0; i < Count; ++i)
        {
            var privateIndex = GetIndex(i);
            yield return _data[privateIndex];
        }
    }
}

public static class LoopyList
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this LoopyList<T>? list) => list is null || list.IsEmpty;
}
