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

    public int Length { get; private set; }
    public int Capacity => _array.Length;
    public Span<byte> Span => _array.AsSpan(0, Length);

    private void Grow(int minCapacity)
    {
        var newCapacity = Math.Max(Capacity * 2, 8);

        while (newCapacity < minCapacity)
            newCapacity *= 2;

        Array.Resize(ref _array, newCapacity);
    }

    public Span<byte> ReserveRaw(int length)
    {
        ThrowIfNegative(length);
        var minCapacity = Length + length;
        if (Capacity < minCapacity)
            Grow(minCapacity);
        var result = _array.AsSpan(Length, length);
        Length += length;
        return result;
    }

    public void Reset() => Length = 0;
}