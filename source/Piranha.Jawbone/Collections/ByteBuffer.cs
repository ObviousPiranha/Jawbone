using System;

namespace Piranha.Jawbone;

public sealed class ByteBuffer
{
    private static void ThrowIfNegative(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
    }

    private byte[] _array = Array.Empty<byte>();
    private readonly int _initialCapacity;

    public int Length { get; private set; }
    public int Capacity => _array.Length;
    public Span<byte> Span => _array.AsSpan(0, Length);

    public ByteBuffer()
    {
        _initialCapacity = 256;
    }

    public ByteBuffer(int initialCapacity)
    {
        if (initialCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Must be at least 1.");
        _initialCapacity = initialCapacity;
    }

    private void Grow(int minCapacity)
    {
        var newCapacity = int.Max(Capacity * 2, _initialCapacity);

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

    public ByteBuffer Reset()
    {
        Length = 0;
        return this;
    }
}
