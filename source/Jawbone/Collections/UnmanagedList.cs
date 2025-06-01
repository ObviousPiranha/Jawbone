using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone;

[DebuggerTypeProxy(typeof(UnmanagedListDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
public sealed class UnmanagedList<T> : IUnmanagedList where T : unmanaged
{
    private T[] _items = [];
    private int _nextCapacity;
    private readonly bool _pinned;

    public bool IsEmpty => Count == 0;
    public int Capacity => _items.Length;
    public int Count { get; private set; }
    public int Size => Count * Unsafe.SizeOf<T>();
    public Span<byte> Bytes => MemoryMarshal.AsBytes(AsSpan());

    public ref T this[int index] => ref AsSpan()[index];
    public ref T this[Index index] => ref AsSpan()[index];
    public Span<T> this[Range range] => AsSpan()[range];

    public UnmanagedList(bool pinned = false)
    {
        _nextCapacity = 64;
        _pinned = pinned;
    }

    public UnmanagedList(int firstCapacity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(firstCapacity, 1);
        _nextCapacity = firstCapacity;
    }

    public void Clear()
    {
        Count = 0;
    }

    public Span<T> Acquire(int count)
    {
        var result = AcquireUninitialized(count);
        result.Clear();
        return result;
    }

    public Span<T> AcquireUninitialized(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        if (count == 0)
            return default;

        EnsureCapacity(Count + count);
        var result = _items.AsSpan(Count, count);
        Count += count;
        return result;
    }

    public void Add(T item)
    {
        if (Count == Capacity)
            Grow();

        _items[Count++] = item;
    }

    public void Add(T item0, T item1)
    {
        EnsureCapacity(Count + 2);
        _items[Count++] = item0;
        _items[Count++] = item1;
    }

    public void Add(T item0, T item1, T item2)
    {
        EnsureCapacity(Count + 3);
        _items[Count++] = item0;
        _items[Count++] = item1;
        _items[Count++] = item2;
    }

    public void Add(T item0, T item1, T item2, T item3)
    {
        EnsureCapacity(Count + 4);
        _items[Count++] = item0;
        _items[Count++] = item1;
        _items[Count++] = item2;
        _items[Count++] = item3;
    }

    public void AddAll(ReadOnlySpan<T> items)
    {
        var minCapacity = Count + items.Length;
        EnsureCapacity(minCapacity);
        items.CopyTo(_items.AsSpan(Count));
        Count = minCapacity;
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
        EnsureCapacity(Count + 1);
        AsSpan(index).CopyTo(_items.AsSpan(index + 1));
        _items[index] = item;
        ++Count;
    }

    public void InsertAll(int index, ReadOnlySpan<T> items)
    {
        EnsureCapacity(Count + items.Length);
        AsSpan(index).CopyTo(_items.AsSpan(index + items.Length));
        items.CopyTo(_items.AsSpan(index));
        Count += items.Length;
    }

    public void RemoveAt(Index index)
    {
        var offset = index.GetOffset(Count);
        AsSpan(offset + 1).CopyTo(_items.AsSpan(offset));
        --Count;
    }

    public void RemoteAt(Range range)
    {
        var (start, count) = range.GetOffsetAndLength(Count);
        AsSpan(start + count).CopyTo(_items.AsSpan(start));
        Count -= count;
    }

    public void RemoveAt(int index, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        AsSpan(index + count).CopyTo(_items.AsSpan(index));
        Count -= count;
    }

    public T Pop()
    {
        var item = _items[Count - 1]; // Ensure throw happens without altering count.
        --Count;
        return item;
    }

    public Span<T> AsSpan() => _items.AsSpan(0, Count);
    public Span<T> AsSpan(int start) => _items.AsSpan(start, Count - start);
    public Span<T> AsSpan(int start, int length) => AsSpan().Slice(start, length);
    public Span<T> AsSpan(Range range) => AsSpan()[range];

    private void AddEnumerable(IEnumerable<T> enumerable, int count)
    {
        var minCapacity = Count + count;
        EnsureCapacity(minCapacity);

        // This is a defensive maneuver against a badly implemented collection
        // where the reported count fails to match the actual number of items
        // in the collection.
        using var enumerator = enumerable.GetEnumerator();
        while (Count < minCapacity && enumerator.MoveNext())
        {
            var current = enumerator.Current;
            _items[Count++] = current;
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
        var items = GC.AllocateUninitializedArray<T>(_nextCapacity, _pinned);
        _nextCapacity *= 2;
        AsSpan().CopyTo(items);
        _items = items;
    }
}
