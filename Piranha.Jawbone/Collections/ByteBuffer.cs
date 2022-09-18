using System;

namespace Piranha.Jawbone.Collections;

public sealed class ByteBuffer
{
    private static void ThrowIfNegative(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
    }

    private byte[] _array = Array.Empty<byte>();

    public int Count { get; private set; }
    public int Capacity => _array.Length;
    public Span<byte> Span => _array;

    private void Grow(int minCapacity)
    {
        var newCapacity = Math.Max(Capacity * 2, 8);

        while (newCapacity < minCapacity)
            newCapacity *= 2;
        
        var array = new byte[newCapacity];
        Span.CopyTo(array);
        _array = array;
    }

    public Span<byte> ReserveRaw(int length)
    {
        ThrowIfNegative(length);
        var minCapacity = Count + length;
        if (Capacity < minCapacity)
            Grow(minCapacity);
        var result = _array.AsSpan(Count, length);
        Count += length;
        return result;
    }

    public void Reset() => Count = 0;
}