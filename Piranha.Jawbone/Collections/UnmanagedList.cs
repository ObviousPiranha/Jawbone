using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Collections;

public sealed class UnmanagedList<T> where T : unmanaged
{
    private T[] _items;

    public int Capacity => _items.Length;
    public int Count { get; private set; }
    public Span<T> Span => _items.AsSpan(0, Count);

    public Span<byte> ByteSpan
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
        _items = new T[16];
    }

    public UnmanagedList(int initialCount)
    {
        if (initialCount < 1)
            throw new ArgumentOutOfRangeException(nameof(initialCount), "Must be at least 1.");
        _items = new T[initialCount];
    }

    public UnmanagedList<T> Clear()
    {
        Count = 0;
        return this;
    }

    public UnmanagedList<T> Add(T item)
    {
        if (Count == Capacity)
            Array.Resize(ref _items, Capacity * 2);
        _items[Count++] = item;
        return this;
    }

    public UnmanagedList<T> Add(ReadOnlySpan<T> items)
    {
        var minCapacity = Count + items.Length;
        if (Capacity < minCapacity)
            Grow(minCapacity);
        
        items.CopyTo(_items.AsSpan(Count));
        Count = minCapacity;
        return this;
    }

    private void Grow(int minCapacity)
    {
        var newCapacity = Capacity * 2;

        while (newCapacity < minCapacity)
            newCapacity *= 2;
        
        Array.Resize(ref _items, newCapacity);
    }
}
