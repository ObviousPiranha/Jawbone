using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[DebuggerTypeProxy(typeof(UnmanagedListDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
public sealed class UnmanagedList<T> : IUnmanagedList where T : unmanaged
{
    private T[] _items = Array.Empty<T>();
    private int _nextCapacity;
    private int _count;

    public bool IsEmpty => _count == 0;
    public int Capacity => _items.Length;
    public int Count => _count;
    public Span<byte> Bytes => MemoryMarshal.AsBytes(AsSpan());

    public ref T this[int index]
    {
        get
        {
            if (_count <= index)
                throw new ArgumentOutOfRangeException(nameof(index));

            return ref _items[index];
        }
    }

    public UnmanagedList()
    {
        _nextCapacity = 64;
    }

    public UnmanagedList(int firstCapacity)
    {
        if (firstCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(firstCapacity), "Must be at least 1.");
        _nextCapacity = firstCapacity;
    }

    public void Clear()
    {
        _count = 0;
    }

    public Span<T> Acquire(int count)
    {
        var result = AcquireUninitialized(count);
        result.Clear();
        return result;
    }

    public Span<T> AcquireUninitialized(int count)
    {
        if (count < 0)
            throw new ArgumentException("Count cannot be negative.", nameof(count));

        if (count == 0)
            return default;

        EnsureCapacity(_count + count);
        var result = _items.AsSpan(_count, count);
        _count += count;
        return result;
    }

    public void Add(T item)
    {
        if (_count == Capacity)
            Grow();

        _items[_count++] = item;
    }

    public void Add(T item0, T item1)
    {
        EnsureCapacity(_count + 2);
        _items[_count++] = item0;
        _items[_count++] = item1;
    }

    public void Add(T item0, T item1, T item2)
    {
        EnsureCapacity(_count + 3);
        _items[_count++] = item0;
        _items[_count++] = item1;
        _items[_count++] = item2;
    }

    public void Add(T item0, T item1, T item2, T item3)
    {
        EnsureCapacity(_count + 4);
        _items[_count++] = item0;
        _items[_count++] = item1;
        _items[_count++] = item2;
        _items[_count++] = item3;
    }

    public void AddAll(ReadOnlySpan<T> items)
    {
        var minCapacity = _count + items.Length;
        EnsureCapacity(minCapacity);
        items.CopyTo(_items.AsSpan(_count));
        _count = minCapacity;
    }

    public void AddRange(IEnumerable<T> items)
    {
        if (items is T[] array)
        {
            AddAll(array);
        }
        else if (items is List<T> list)
        {
            var span = CollectionsMarshal.AsSpan(list);
            AddAll(span);
        }
        else if (items is ImmutableArray<T> immutableArray)
        {
            AddAll(immutableArray.AsSpan());
        }
        else if (items is IList<T> ilist)
        {
            AddEnumerable(items, ilist.Count);
        }
        else if (items is IReadOnlyList<T> readOnlyList)
        {
            AddEnumerable(items, readOnlyList.Count);
        }
        else if (items is ICollection<T> collection)
        {
            AddEnumerable(items, collection.Count);
        }
        else if (items is IReadOnlyCollection<T> readOnlyCollection)
        {
            AddEnumerable(items, readOnlyCollection.Count);
        }
        else
        {
            foreach (var item in items)
                Add(item);
        }
    }

    public void Insert(int index, T item)
    {
        EnsureCapacity(_count + 1);
        AsSpan(index).CopyTo(_items.AsSpan(index + 1));
        _items[index] = item;
        ++_count;
    }

    public void InsertAll(int index, ReadOnlySpan<T> items)
    {
        EnsureCapacity(_count + items.Length);
        AsSpan(index).CopyTo(_items.AsSpan(index + items.Length));
        items.CopyTo(_items.AsSpan(index));
        _count += items.Length;
    }

    public void RemoveAt(int index)
    {
        AsSpan(index + 1).CopyTo(_items.AsSpan(index));
        --_count;
    }

    public void RemoveAt(int index, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));
        AsSpan(index + count).CopyTo(_items.AsSpan(index));
        _count -= count;
    }

    public T Pop()
    {
        var item = _items[_count - 1]; // Ensure throw happens without altering count.
        --_count;
        return item;
    }

    public Span<T> AsSpan() => _items.AsSpan(0, _count);
    public Span<T> AsSpan(int start) => _items.AsSpan(start, _count - start);
    public Span<T> AsSpan(int start, int length) => AsSpan().Slice(start, length);

    private void AddEnumerable(IEnumerable<T> enumerable, int count)
    {
        var minCapacity = _count + count;
        EnsureCapacity(minCapacity);

        // This is a defensive maneuver against a badly implemented collection
        // where the reported count fails to match the actual number of items
        // in the collection.
        using var enumerator = enumerable.GetEnumerator();
        while (_count < minCapacity && enumerator.MoveNext())
        {
            var current = enumerator.Current;
            _items[_count++] = current;
        }
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (Capacity < minCapacity)
            Grow(minCapacity);
    }

    private void Grow(int minCapacity)
    {
        while (_nextCapacity < minCapacity)
            _nextCapacity *= 2;

        Grow();
    }

    private void Grow()
    {
        var items = new T[_nextCapacity];
        _nextCapacity *= 2;
        AsSpan().CopyTo(items);
        _items = items;
    }

    public static T[] CreateArray(int length, SpanAction<byte> action)
    {
        var result = new T[length];
        var span = MemoryMarshal.AsBytes(result.AsSpan());
        action.Invoke(span);
        return result;
    }

    public static T[] CreateArray<TState>(int length, TState state, SpanAction<byte, TState> action)
    {
        var result = new T[length];
        var span = MemoryMarshal.AsBytes(result.AsSpan());
        action.Invoke(span, state);
        return result;
    }
}
