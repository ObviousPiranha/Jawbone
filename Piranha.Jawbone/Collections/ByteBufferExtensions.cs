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
        var byteCount = items.Length * Unsafe.SizeOf<T>();
        var reserved = byteBuffer.ReserveRaw(byteCount);

        unsafe
        {
            fixed (void* source = items)
            fixed (void* destination = reserved)
            {
                Buffer.MemoryCopy(
                    source,
                    destination,
                    reserved.Length,
                    byteCount);
            }
        }

        return byteBuffer;
    }

    public static ByteBuffer AddAsBytes<T>(
        this ByteBuffer byteBuffer,
        in T item) where T : unmanaged
    {
        var reserved = byteBuffer.ReserveRaw(Unsafe.SizeOf<T>());

        unsafe
        {
            fixed (void* a = &item)
            {
                var bytes = new ReadOnlySpan<byte>(a, Unsafe.SizeOf<T>());
                bytes.CopyTo(reserved);
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