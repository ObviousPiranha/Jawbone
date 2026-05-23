using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Jawbone;

public sealed class LoopyList<T>
{
    private T[] _data = [];
    private int _begin;

    public int Count { get; private set; }
    public int Capacity => _data.Length;
    public int Free => Capacity - Count;
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

    public bool IsContiguous => (_begin + Count) <= Capacity;

    public DualSpan<T> AsSpan()
    {
        var end = GetEnd(Count);
        if (end < _begin)
        {
            var result = new DualSpan<T>(
                _data.AsSpan(_begin),
                _data.AsSpan(0, end));
            return result;
        }
        else
        {
            var result = new DualSpan<T>(
                _data.AsSpan(_begin..end));
            return result;
        }
    }

    public DualSpan<T> AsSpan(Range range)
    {
        var (start, count) = range.GetOffsetAndLength(Count);
        return AsSpan(start, count);
    }

    public DualSpan<T> AsSpan(int start)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, Count);
        var begin = GetBegin(start);
        var end = GetEnd(Count);
        if (end < begin)
        {
            var result = new DualSpan<T>(
                _data.AsSpan(begin),
                _data.AsSpan(0, end));
            return result;
        }
        else
        {
            var result = new DualSpan<T>(
                _data.AsSpan(begin..end));
            return result;
        }
    }

    public DualSpan<T> AsSpan(int start, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, Count);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start + count, Count);
        var begin = GetBegin(start);
        var end = GetEnd(start + count);
        if (end < begin)
        {
            var result = new DualSpan<T>(
                _data.AsSpan(begin),
                _data.AsSpan(0, end));
            return result;
        }
        else
        {
            var result = new DualSpan<T>(
                _data.AsSpan(begin..end));
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
        var privateIndex = GetBegin(Count++);
        _data[privateIndex] = item;
    }

    public void PushBack(params ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
            return;
        EnsureCapacityFor(items.Length);
        var end = GetEnd(Count);
        var free = Capacity - end;
        if (items.Length <= free)
        {
            items.CopyTo(_data.AsSpan(end));
        }
        else
        {
            items[..free].CopyTo(_data.AsSpan(end));
            items[free..].CopyTo(_data);
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
        if (Count == 0)
            return;
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
        _begin = --Count == 0 ? 0 : GetBegin(1);
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
            _begin = GetBegin(count);
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

    public void Expand() => Grow(0);

    private void EnsureCapacityFor(int count)
    {
        var freeCapacity = Capacity - Count;
        if (count <= freeCapacity)
            return;
        var maxFreeCapacity = int.MaxValue - Count;
        if (maxFreeCapacity < count)
            Throw();
        Grow(Count + count);
        
        [DoesNotReturn] static void Throw() =>
            throw new InvalidOperationException("Not enough room for this operation.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int minCapacity)
    {
        var nextCapacity = int.Max(Capacity * 2, 16);
        while (nextCapacity < minCapacity)
            nextCapacity *= 2;
        Debug.Assert((nextCapacity & (nextCapacity - 1)) == 0);

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
        private readonly LoopyList<T> _list;
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
