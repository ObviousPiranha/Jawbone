using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Collections;

public sealed class UnmanagedList<T> : IUnmanagedList where T : unmanaged
{
    private T[] _items = Array.Empty<T>();
    private int _nextCapacity;
    private int _count;

    public bool IsEmpty => _count == 0;
    public int Capacity => _items.Length;
    public int Count => _count;
    public Span<T> Items => _items.AsSpan(0, Count);
    public Span<byte> Bytes
    {
        get
        {
            var length = Count * Unsafe.SizeOf<T>();

            unsafe
            {
                fixed (void* pointer = _items)
                    return new Span<byte>(pointer, length);
            }
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

    public void Add(T item)
    {
        if (_count == Capacity)
        {
            Array.Resize(ref _items, _nextCapacity);
            _nextCapacity *= 2;
        }

        _items[_count++] = item;
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
        Items.Slice(index).CopyTo(_items.AsSpan(index + 1));
        _items[index] = item;
        ++_count;
    }

    public void InsertAll(int index, ReadOnlySpan<T> items)
    {
        EnsureCapacity(_count + items.Length);
        Items.Slice(index).CopyTo(_items.AsSpan(index + items.Length));
        items.CopyTo(_items.AsSpan(index));
        _count += items.Length;
    }

    public void RemoveAt(int index)
    {
        Items.Slice(index + 1).CopyTo(_items.AsSpan(index));
        --_count;
    }

    public void RemoveAt(int index, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));
        Items.Slice(index + count).CopyTo(_items.AsSpan(index));
        _count -= count;
    }

    public T Pop()
    {
        var item = _items[_count - 1]; // Ensure throw happens without altering count.
        --_count;
        return item;
    }

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

        Array.Resize(ref _items, _nextCapacity);
        _nextCapacity *= 2;
    }
}
