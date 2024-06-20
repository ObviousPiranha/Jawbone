using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public ref struct SpanWriter<T>
{
    public Span<T> Span;
    public int Position;

    public readonly Span<T> Free => Span.Slice(Position);
    public readonly ReadOnlySpan<T> Written => Span.Slice(0, Position);

    public SpanWriter(Span<T> span) => Span = span;
}

public static class SpanWriterExtensions
{
    public static SpanWriter<T> ToWriter<T>(this Span<T> span) => new(span);
    public static SpanWriter<T> ToWriter<T>(this T[] array) => new(array);

    public static ref SpanWriter<T> Write<T>(
        ref this SpanWriter<T> writer,
        T value)
    {
        writer.Free[0] = value;
        ++writer.Position;
        return ref writer;
    }

    public static ref SpanWriter<T> Write<T>(
        ref this SpanWriter<T> writer,
        ReadOnlySpan<T> values)
    {
        values.CopyTo(writer.Free);
        writer.Position += values.Length;
        return ref writer;
    }

    public static ref SpanWriter<byte> Blit<T>(
        ref this SpanWriter<byte> writer,
        in T value
    ) where T : unmanaged
    {
        MemoryMarshal.Write(writer.Free, value);
        writer.Position += Unsafe.SizeOf<T>();
        return ref writer;
    }

    public static ref SpanWriter<byte> Blit<T>(
        ref this SpanWriter<byte> writer,
        ReadOnlySpan<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        bytes.CopyTo(writer.Free);
        writer.Position += bytes.Length;
        return ref writer;
    }
}
