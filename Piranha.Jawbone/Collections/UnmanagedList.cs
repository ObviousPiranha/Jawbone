using System;
using System.Collections.Generic;
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

    public UnmanagedList<T> Clear()
    {
        _count = 0;
        return this;
    }

    public UnmanagedList<T> Add(T item)
    {
        if (_count == Capacity)
        {
            Array.Resize(ref _items, _nextCapacity);
            _nextCapacity *= 2;
        }

        _items[_count++] = item;
        return this;
    }

    public UnmanagedList<T> AddAll(ReadOnlySpan<T> items)
    {
        var minCapacity = _count + items.Length;
        EnsureCapacity(minCapacity);
        items.CopyTo(_items.AsSpan(_count));
        _count = minCapacity;
        return this;
    }

    public UnmanagedList<T> AddRange(IEnumerable<T> items)
    {
        if (items is List<T> list)
        {
            var span = CollectionsMarshal.AsSpan(list);
            return AddAll(span);
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

        return this;
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
