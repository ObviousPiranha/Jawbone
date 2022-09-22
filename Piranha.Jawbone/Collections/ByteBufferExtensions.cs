using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Collections;

public static class ByteBufferExtensions
{
    public static ByteBuffer AddBytes(
        this ByteBuffer byteBuffer,
        ReadOnlySpan<byte> bytes)
    {
        bytes.CopyTo(byteBuffer.ReserveRaw(bytes.Length));
        return byteBuffer;
    }

    public static ByteBuffer AddAllAsBytes<T>(
        this ByteBuffer byteBuffer,
        T[] items) where T : unmanaged
    {
        return byteBuffer.AddAllAsBytes<T>(items.AsSpan());
    }

    public static ByteBuffer AddAllAsBytes<T>(
        this ByteBuffer byteBuffer,
        ReadOnlySpan<T> items) where T : unmanaged
    {
        var sourceByteCount = items.Length * Unsafe.SizeOf<T>();
        var destination = byteBuffer.ReserveRaw(sourceByteCount);

        unsafe
        {
            fixed (void* sourcePointer = items)
            {
                var source = new ReadOnlySpan<byte>(sourcePointer, sourceByteCount);
                source.CopyTo(destination);
            }
        }

        return byteBuffer;
    }

    public static ByteBuffer AddAsBytes<T>(
        this ByteBuffer byteBuffer,
        in T item) where T : unmanaged
    {
        var destination = byteBuffer.ReserveRaw(Unsafe.SizeOf<T>());

        unsafe
        {
            fixed (void* a = &item)
            {
                var source = new ReadOnlySpan<byte>(a, Unsafe.SizeOf<T>());
                source.CopyTo(destination);
            }
        }

        return byteBuffer;
    }

    public static Span<byte> Reserve(
        this ByteBuffer byteBuffer,
        int length,
        byte defaultValue = default)
    {
        var result = byteBuffer.ReserveRaw(length);
        result.Fill(defaultValue);
        return result;
    }
}
