using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Extensions;

public static class StreamExtensions
{
    public static T ReadValue<T>(this Stream stream) where T : unmanaged
    {
        Unsafe.SkipInit(out T result);
        var n = stream.Read(
            MemoryMarshal.AsBytes(
                new Span<T>(ref result)));
        if (n != Unsafe.SizeOf<T>())
            throw new InvalidDataException($"Insufficient bytes to read into {typeof(T)}");
        return result;
    }

    public static bool TryReadValue<T>(this Stream stream, out T value) where T : unmanaged
    {
        Unsafe.SkipInit(out value);
        var n = stream.Read(
            MemoryMarshal.AsBytes(
                new Span<T>(ref value)));
        return n == Unsafe.SizeOf<T>();
    }

    public static Stream WriteValue<T>(this Stream stream, in T value) where T : unmanaged
    {
        stream.Write(
            MemoryMarshal.AsBytes(
                new ReadOnlySpan<T>(in value)));
        return stream;
    }
}
