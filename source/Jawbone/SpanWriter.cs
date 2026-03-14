using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone;

public ref struct SpanWriter<T>
{
    public readonly Span<T> Span;
    public int Position;

    public readonly Span<T> Free => Span.Slice(Position);
    public readonly ReadOnlySpan<T> Written => Span.Slice(0, Position);
    public readonly bool IsFull => Position == Span.Length;

    public SpanWriter(Span<T> span) => Span = span;
}

public static class SpanWriter
{
    public static SpanWriter<T> Create<T>(Span<T> span) => new(span);
    public static SpanWriter<T> Create<T>(Memory<T> memory) => new(memory.Span);
    public static SpanWriter<T> Create<T>(T[]? array) => new(array);
    public static SpanWriter<T> Create<T>(List<T>? list) => new(CollectionsMarshal.AsSpan(list));
    public static SpanWriter<T> Create<T>(ArraySegment<T> segment) => new(segment);
    public static unsafe SpanWriter<T> Create<T>(nint ptr, int length) => new(new(ptr.ToPointer(), length));

    public static SpanWriter<T> FromBytes<T>(Span<byte> bytes) where T : unmanaged
    {
        // Workaround for .NET 10 issue.
        var span = MemoryMarshal.Cast<byte, T>(bytes);
        return new(span);
    }

    public static void Write<T>(
        ref this SpanWriter<T> writer,
        T value)
    {
        writer.Span[writer.Position] = value;
        ++writer.Position;
    }

    public static void Write<T>(
        ref this SpanWriter<T> writer,
        scoped ReadOnlySpan<T> values)
    {
        values.CopyTo(writer.Free);
        writer.Position += values.Length;
    }

    public static void Write<T>(
        ref this SpanWriter<T> writer,
        scoped Span<T> values)
    {
        values.CopyTo(writer.Free);
        writer.Position += values.Length;
    }

    public static bool TryWrite<T>(
        ref this SpanWriter<T> writer,
        T value)
    {
        if (writer.Span.Length <= writer.Position)
            return false;
        writer.Span[writer.Position++] = value;
        return true;
    }

    public static bool TryWrite<T>(
        ref this SpanWriter<T> writer,
        scoped ReadOnlySpan<T> values)
    {
        if (!values.TryCopyTo(writer.Free))
            return false;
        writer.Position += values.Length;
        return true;
    }

    public static bool TryWrite<T>(
        ref this SpanWriter<T> writer,
        scoped Span<T> values)
    {
        if (!values.TryCopyTo(writer.Free))
            return false;
        writer.Position += values.Length;
        return true;
    }

    public static void Blit<T>(
        ref this SpanWriter<byte> writer,
        in T value
    ) where T : unmanaged
    {
        MemoryMarshal.Write(writer.Free, value);
        writer.Position += Unsafe.SizeOf<T>();
    }

    public static void Blit<T>(
        ref this SpanWriter<byte> writer,
        scoped ReadOnlySpan<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        bytes.CopyTo(writer.Free);
        writer.Position += bytes.Length;
    }

    public static void Blit<T>(
        ref this SpanWriter<byte> writer,
        scoped Span<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        bytes.CopyTo(writer.Free);
        writer.Position += bytes.Length;
    }

    public static bool TryBlit<T>(
        ref this SpanWriter<byte> writer,
        in T value
    ) where T : unmanaged
    {
        if (!MemoryMarshal.TryWrite(writer.Free, value))
            return false;
        writer.Position += Unsafe.SizeOf<T>();
        return true;
    }

    public static bool TryBlit<T>(
        ref this SpanWriter<byte> writer,
        scoped ReadOnlySpan<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        if (!bytes.TryCopyTo(writer.Free))
            return false;
        writer.Position += bytes.Length;
        return true;
    }

    public static bool TryBlit<T>(
        ref this SpanWriter<byte> writer,
        scoped Span<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        if (!bytes.TryCopyTo(writer.Free))
            return false;
        writer.Position += bytes.Length;
        return true;
    }

    public static bool TryFormat<T>(
        ref this SpanWriter<byte> writer,
        T value,
        ReadOnlySpan<char> format = default,
        IFormatProvider? formatProvider = default
        ) where T : IUtf8SpanFormattable
    {
        if (!value.TryFormat(writer.Free, out var bytesWritten, format, formatProvider))
            return false;
        writer.Position += bytesWritten;
        return true;
    }

    public static bool TryFormat<T>(
        ref this SpanWriter<char> writer,
        T value,
        ReadOnlySpan<char> format = default,
        IFormatProvider? formatProvider = default
        ) where T : ISpanFormattable
    {
        if (!value.TryFormat(writer.Free, out var charsWritten, format, formatProvider))
            return false;
        writer.Position += charsWritten;
        return true;
    }
}
