using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone;

public sealed class LoopyList<T>
{
    private T[] _data = [];
    private int _begin;

    public int Count { get; private set; }
    public int Capacity => _data.Length;
    public bool IsEmpty => Count == 0;
    private int Mask => Capacity - 1;

    private int GetBegin(int offset) => (_begin + offset) & Mask;
    private int GetEnd(int offset)
    {
        var result = _begin + offset;
        if (result != Capacity)
            result &= Mask;
        return result;
    }

    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
            var privateIndex = GetBegin(index);
            var result = _data[privateIndex];
            return result;
        }

        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
            var privateIndex = GetBegin(index);
            _data[privateIndex] = value;
        }
    }

    public T this[Index index]
    {
        get => this[index.GetOffset(Count)];
        set => this[index.GetOffset(Count)] = value;
    }

    public bool IsContiguous
    {
        get
        {
            var end = _begin + Count;
            return end <= Capacity;
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
        var privateIndex = GetBegin(Count++);
        _data[privateIndex] = item;
    }

    public void PushBack(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacityFor(items.Length);
        var free = GetBegin(Count);
        var buffer = free < _begin ? _data.AsSpan(free.._begin) : _data.AsSpan(free..);
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
        EnsureCapacityFor(1);
        var privateIndex = GetBegin(-1);
        _data[privateIndex] = item;
        _begin = privateIndex;
        ++Count;
    }

    public void PushFront(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacityFor(items.Length);
        var free = GetBegin(Count);
        var buffer = free < _begin ? _data.AsSpan(free.._begin) : _data.AsSpan(.._begin);
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
        _begin = GetBegin(-items.Length);
        Count += items.Length;
    }

    public T PopBack()
    {
        if (Count == 0)
            throw new InvalidOperationException("List is empty.");

        var last = GetBegin(Count - 1);
        var result = _data[last];
        if (--Count == 0)
            _begin = 0;
        return result;
    }

    public void PopBackWhile(Predicate<T> predicate) => PopBackWhile(predicate, static (item, state) => state.Invoke(item));
    public void PopBackWhile<TState>(TState arg, Func<T, TState, bool> predicate)
    {
        var last = GetBegin(Count - 1);
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
        _begin = GetBegin(1);
        return true;
    }

    public T PopFront()
    {
        if (Count == 0)
            throw new InvalidOperationException("List is empty.");

        var result = _data[_begin];
        _begin = --Count == 0 ? 0 : GetBegin(1);
        return result;
    }

    public void PopFrontWhile(Predicate<T> predicate) => PopFrontWhile(predicate, static (item, state) => state.Invoke(item));
    public void PopFrontWhile<TState>(TState arg, Func<T, TState, bool> predicate)
    {
        while (0 < Count && predicate.Invoke(_data[_begin], arg))
        {
            _begin = GetBegin(1);
            --Count;
        }

        if (Count == 0)
            _begin = 0;
    }

    public void CopyTo(Span<T> destination)
    {
        var end = GetEnd(Count);
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

    public void CopyTo(Range sourceRange, Span<T> destination)
    {
        var (start, count) = sourceRange.GetOffsetAndLength(Count);

        var begin = GetBegin(start);
        var end = GetEnd(start + count);

        if (end < begin)
        {
            var first = _data.AsSpan(begin..);
            first.CopyTo(destination);
            _data.AsSpan(..end).CopyTo(destination[first.Length..]);
        }
        else
        {
            _data.AsSpan(begin..end).CopyTo(destination);
        }
    }

    public T[] ToArray()
    {
        var result = new T[Count];
        CopyTo(result);
        return result;
    }

    public bool SequenceEqual(params ReadOnlySpan<T> items)
    {
        var end = GetEnd(items.Length);

        if (end < _begin)
        {
            if (Count != items.Length)
                return false;

            var first = _data.AsSpan(_begin..);

            return
                first.SequenceEqual(items[..first.Length]) &&
                _data.AsSpan(..end).SequenceEqual(items[first.Length..]);
        }
        else
        {
            return _data.AsSpan(_begin..end).SequenceEqual(items);
        }
    }

    private void EnsureCapacityFor(int additionalItemCount)
    {
        var minCapacity = Count + additionalItemCount;
        if (Capacity < minCapacity)
            Grow(minCapacity);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int minCapacity)
    {
        var nextCapacity = int.Max(Capacity * 2, 16);
        while (nextCapacity < minCapacity)
            nextCapacity *= 2;

        var data = new T[nextCapacity];
        var end = GetEnd(Count);
        if (end < _begin)
        {
            var first = _data.AsSpan(_begin);
            first.CopyTo(data);
            _data.AsSpan(0, end).CopyTo(data.AsSpan(first.Length));
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
            var privateIndex = GetBegin(i);
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
