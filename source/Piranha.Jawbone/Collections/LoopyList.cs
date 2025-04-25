using System;
using System.Collections;
using System.Collections.Generic;

namespace Piranha.Jawbone;

public sealed class LoopyList<T>
{
    private T[] _data = [];
    private int _begin;

    public int Count { get; private set; }
    public int Capacity => _data.Length;
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
        get => this[index.IsFromEnd ? Count - index.Value : index.Value];
        set => this[index.IsFromEnd ? Count - index.Value : index.Value] = value;
    }

    public void Clear()
    {
        _begin = 0;
        Count = 0;
    }

    public void PushBack(T item)
    {
        EnsureCapacity(1);
        var privateIndex = GetIndex(Count++);
        _data[privateIndex] = item;
    }

    public void PushBack(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacity(items.Length);
        var end = GetIndex(Count);
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
        EnsureCapacity(1);
        var privateIndex = GetIndex(-1);
        _data[privateIndex] = item;
        _begin = privateIndex;
        ++Count;
    }

    public void PushFront(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacity(items.Length);
        var end = GetIndex(Count);
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
        _begin = GetIndex(-items.Length);
        Count += items.Length;
    }

    public T PopBack()
    {
        if (Count == 0)
            throw new InvalidOperationException("Circular list is empty.");

        var last = GetIndex(Count - 1);
        var result = _data[last];
        if (--Count == 0)
            _begin = 0;
        return result;
    }

    public void PopBackWhile(Predicate<T> predicate)
    {
        var last = GetIndex(Count - 1);
        while (0 < Count && predicate.Invoke(_data[last]))
        {
            last = (last - 1) & Mask;
            --Count;
        }

        if (Count == 0)
            _begin = 0;
    }

    public T PopFront()
    {
        if (Count == 0)
            throw new InvalidOperationException("Circular list is empty.");

        var result = _data[_begin];
        _begin = --Count == 0 ? 0 : GetIndex(1);
        return result;
    }

    public void PopFrontWhile(Predicate<T> predicate)
    {
        while (0 < Count && predicate.Invoke(_data[_begin]))
        {
            _begin = GetIndex(1);
            --Count;
        }

        if (Count == 0)
            _begin = 0;
    }

    public void CopyTo(Span<T> destination)
    {
        var end = GetIndex(Count);
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

    private void EnsureCapacity(int additionalItemCount)
    {
        var minCapacity = Count + additionalItemCount;
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

    public IEnumerable<T> AsEnumerable()
    {
        for (int i = 0; i < Count; ++i)
        {
            var privateIndex = GetIndex(i);
            yield return _data[privateIndex];
        }
    }

    public Enumerator GetEnumerator() => new(this);

    public struct Enumerator
    {
        private LoopyList<T> _list;
        private int _index;

        public Enumerator(LoopyList<T> list)
        {
            _list = list;
            Current = default!;
        }

        public T Current { get; private set; }

        public bool MoveNext()
        {
            if (_index < _list.Count)
            {
                Current = _list[_index++];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
