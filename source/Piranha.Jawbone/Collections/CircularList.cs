using System;

namespace Piranha.Jawbone;

public sealed class CircularList<T>
{
    private T[] _data = [];
    private int _begin;

    public int Count { get; private set; }
    public int Capacity => _data.Length;
    private int Mask => Capacity - 1;

    private int GetPrivateIndex(int index) => (_begin + index) & Mask;

    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
            var privateIndex = GetPrivateIndex(index);
            var result = _data[privateIndex];
            return result;
        }

        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
            var privateIndex = GetPrivateIndex(index);
            _data[privateIndex] = value;
        }
    }

    public void PushBack(T item)
    {
        EnsureCapacity(Count + 1);
        var privateIndex = GetPrivateIndex(Count++);
        _data[privateIndex] = item;
    }

    public void PushBack(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacity(Count + items.Length);
        var end = (_begin + Count) & Mask;
        var buffer = end < _begin ? _data.AsSpan(end.._begin) : _data.AsSpan(end..);
        if (buffer.Length < items.Length)
        {
            items[..buffer.Length].CopyTo(buffer);
            items[buffer.Length..].CopyTo(_data);
        }
        else
        {
            items.CopyTo(buffer);
        }
        Count += items.Length;
    }

    public void PushFront(T item)
    {
        EnsureCapacity(Count + 1);
        var privateIndex = (_begin - 1) & Mask;
        _data[privateIndex] = item;
        _begin = privateIndex;
        ++Count;
    }

    public void PushFront(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacity(Count + items.Length);
        var end = (_begin + Count) & Mask;
        var buffer = end < _begin ? _data.AsSpan(end.._begin) : _data.AsSpan(.._begin);
        if (buffer.Length < items.Length)
        {
            items[^buffer.Length..].CopyTo(buffer);
            var remaining = items[..^buffer.Length];
            remaining.CopyTo(_data.AsSpan(^remaining.Length..));
        }
        else
        {
            items.CopyTo(buffer[^items.Length..]);
        }
        _begin = (_begin - items.Length) & Mask;
        Count += items.Length;
    }

    public void CopyTo(Span<T> destination)
    {
        var end = (_begin + Count) & Mask;
        if (end < _begin)
        {
            var block = _data.AsSpan(_begin..);
            block.CopyTo(destination);
            _data.AsSpan(..end).CopyTo(destination[block.Length..]);
        }
        else
        {
            _data.AsSpan(_begin..end).CopyTo(destination);
        }
    }

    public T[] ToArray()
    {
        var result = new T[Count];
        CopyTo(result);
        return result;
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (Capacity < minCapacity)
            Grow(minCapacity);
    }

    private void Grow(int minCapacity)
    {
        var nextCapacity = int.Max(Capacity * 2, 16);
        while (nextCapacity < minCapacity)
            nextCapacity *= 2;

        var data = new T[nextCapacity];
        var end = _begin + Count;
        if (Capacity < end)
        {
            var firstBlock = _data.AsSpan(_begin);
            firstBlock.CopyTo(data);
            _data.AsSpan(0, end - Capacity).CopyTo(data.AsSpan(firstBlock.Length));
        }
        else
        {
            _data.AsSpan(_begin..end).CopyTo(data);
        }

        _data = data;
        _begin = 0;
    }
}
