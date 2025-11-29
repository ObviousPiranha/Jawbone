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

    public static void Write<T>(
        ref this SpanWriter<T> writer,
        T value)
    {
        writer.Free[0] = value;
        ++writer.Position;
    }

    public static void Write<T>(
        ref this SpanWriter<T> writer,
        scoped ReadOnlySpan<T> values)
    {
        values.CopyTo(writer.Free);
        writer.Position += values.Length;
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
}
