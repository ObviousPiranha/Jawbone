using System;

namespace Piranha.Jawbone.Collections;

public class CircularBuffer<T>
{
    private readonly T[] _items;

    private int _startIndex = 0;

    public int Length { get; private set; }
    public int Capacity => _items.Length;
    public int Available => Capacity - Length;

    public CircularBuffer(int capacity) => _items = new T[capacity];

    public int Take(Span<T> items)
    {
        var length = Math.Min(Length, items.Length);
        var endIndex = _startIndex + length;
        if (Capacity < endIndex)
        {
            var firstSegment = _items.AsSpan(_startIndex);
            var secondSegment = _items.AsSpan(0, length - firstSegment.Length);
            
            firstSegment.CopyTo(items);
            secondSegment.CopyTo(items[firstSegment.Length..]);
        }
        else
        {
            _items.AsSpan(_startIndex, length).CopyTo(items);
        }

        _startIndex = (_startIndex + length) % Capacity;
        Length -= length;
        return length;
    }

    public int Add(ReadOnlySpan<T> items)
    {
        var length = Math.Min(Available, items.Length);
        var endIndex = _startIndex + length;

        if (Capacity < endIndex)
        {
            var firstSegmentLength = Capacity - _startIndex;
            items[..firstSegmentLength].CopyTo(_items.AsSpan(_startIndex));
            items[firstSegmentLength..length].CopyTo(_items);
        }
        else
        {
            items[..length].CopyTo(_items.AsSpan(_startIndex));
        }

        Length += length;
        return length;
    }
}
