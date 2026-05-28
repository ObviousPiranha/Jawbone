using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone;

[DebuggerTypeProxy(typeof(UnmanagedListDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
public sealed class UnmanagedList<T> : IUnmanagedList where T : unmanaged
{
    private const int DangerZone = 1 << 30;
    private const int DefaultFirstCapacity = 64;

    private T[] _items = [];
    private readonly bool _pinned;

    public bool IsEmpty => Count == 0;
    public int Capacity => _items.Length;
    public int Count { get; internal set; }
    public int Size => Count * Unsafe.SizeOf<T>();
    public Span<byte> Bytes => MemoryMarshal.AsBytes(AsSpan());
    public Span<T> Items => AsSpan();
    internal Span<T> Free => _items.AsSpan(Count);

    public ref T this[int index] => ref AsSpan()[index];
    public ref T this[Index index] => ref AsSpan()[index];
    public Span<T> this[Range range] => AsSpan(range);

    public UnmanagedList(bool pinned = false)
    {
        _pinned = pinned;
    }

    public UnmanagedList(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        _items = new T[capacity];
    }

    public void Clear() => Count = 0;

    public void Expand() => Grow(Capacity * 2);

    public Span<T> Acquire(int count)
    {
        var result = AcquireUninitialized(count);
        result.Clear();
        return result;
    }

    public Span<T> Acquire(int count, T fillValue)
    {
        var result = AcquireUninitialized(count);
        result.Fill(fillValue);
        return result;
    }

    public Span<T> AcquireUninitialized(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        if (count == 0)
            return default;

        EnsureCapacityFor(count);
        var result = _items.AsSpan(Count, count);
        Count += count;
        return result;
    }

    public void Add(T item)
    {
        EnsureCapacityFor(1);
        _items[Count++] = item;
    }

    public void Add(T item0, T item1)
    {
        EnsureCapacityFor(2);
        _items[Count++] = item0;
        _items[Count++] = item1;
    }

    public void Add(T item0, T item1, T item2)
    {
        EnsureCapacityFor(3);
        _items[Count++] = item0;
        _items[Count++] = item1;
        _items[Count++] = item2;
    }

    public void Add(T item0, T item1, T item2, T item3)
    {
        EnsureCapacityFor(4);
        _items[Count++] = item0;
        _items[Count++] = item1;
        _items[Count++] = item2;
        _items[Count++] = item3;
    }

    public void AddAll(ReadOnlySpan<T> items)
    {
        EnsureCapacityFor(items.Length);
        items.CopyTo(_items.AsSpan(Count));
        Count += items.Length;
    }

    public void AddAll(DualReadOnlySpan<T> items)
    {
        EnsureCapacityFor(items.Length);
        items.CopyTo(_items.AsSpan(Count));
        Count += items.Length;
    }

    public void AddRange(IEnumerable<T> items)
    {
        if (SpanReader.TryGetSpan(items, out var span))
        {
            AddAll(span);
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
        EnsureCapacityFor(1);
        AsSpan(index).CopyTo(_items.AsSpan(index + 1));
        _items[index] = item;
        ++Count;
    }

    public void InsertAll(int index, ReadOnlySpan<T> items)
    {
        EnsureCapacityFor(items.Length);
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

    public void RemoveAt(Range range)
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

    public void RemoveAll<TState>(TState state, Func<T, TState, bool> predicate)
    {
        var free = 0;
        while (free < Count && !predicate.Invoke(_items[free], state))
            ++free;
        for (int i = free + 1; i < Count; ++i)
        {
            if (!predicate.Invoke(_items[i], state))
                _items[free++] = _items[i];
        }
        Count = free;
    }

    public void RemoveAll(Predicate<T> predicate)
    {
        var free = 0;
        while (free < Count && !predicate.Invoke(_items[free]))
            ++free;
        for (int i = free + 1; i < Count; ++i)
        {
            if (!predicate.Invoke(_items[i]))
                _items[free++] = _items[i];
        }
        Count = free;
    }

    public T Pop()
    {
        ThrowIfEmpty();
        var result = _items[--Count];
        return result;
    }

    public void RemoveLast()
    {
        ThrowIfEmpty();
        --Count;
    }

    public Span<T> AsSpan() => _items.AsSpan(0, Count);
    public Span<T> AsSpan(int start) => _items.AsSpan(start, Count - start);
    public Span<T> AsSpan(int start, int length) => AsSpan().Slice(start, length);
    public Span<T> AsSpan(Range range) => AsSpan()[range];

    private void AddEnumerable(IEnumerable<T> enumerable, int count)
    {
        EnsureCapacityFor(count);
        var maxCount = Count + count;

        // This is a defensive maneuver against a badly implemented collection
        // where the reported count fails to match the actual number of items
        // in the collection.
        using var enumerator = enumerable.GetEnumerator();
        while (Count < maxCount && enumerator.MoveNext())
        {
            var current = enumerator.Current;
            _items[Count++] = current;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacityFor(int count)
    {
        var freeCapacity = Capacity - Count;
        if (freeCapacity < count)
            GrowFor(count);
    }

    private void GrowFor(int count)
    {
        var maxFreeCapacity = int.MaxValue - Count;
        if (maxFreeCapacity < count)
            throw new InvalidOperationException("Not enough room for this operation.");
        Grow(Count + count);
    }

    private void Grow(int minCapacity)
    {
        var nextCapacity = 0 < Capacity ? Capacity * 2 : DefaultFirstCapacity;
        while (nextCapacity < minCapacity)
        {
            if (nextCapacity < DangerZone)
            {
                nextCapacity *= 2;
            }
            else
            {
                nextCapacity = int.MaxValue;
                break;
            }
        }

        var items = GC.AllocateUninitializedArray<T>(nextCapacity, _pinned);
        AsSpan().CopyTo(items);
        _items = items;
    }

    private void ThrowIfEmpty()
    {
        if (Count < 1)
            Throw();

        [DoesNotReturn] static void Throw() =>
            throw new InvalidOperationException("Collection is empty.");
    }

    public static implicit operator Span<T>(UnmanagedList<T> list) => list.AsSpan();
    public static implicit operator ReadOnlySpan<T>(UnmanagedList<T> list) => list.AsSpan();
}
