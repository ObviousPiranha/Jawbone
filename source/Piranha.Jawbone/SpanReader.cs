using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public ref struct SpanReader<T>
{
    public ReadOnlySpan<T> Span;
    public int Position;

    public readonly ReadOnlySpan<T> Pending => Span.Slice(Position);
    public readonly ReadOnlySpan<T> Consumed => Span.Slice(0, Position);
    public readonly int RemainingCount => Span.Length - Position;
    public readonly bool WasConsumed => Span.Length <= Position;

    public SpanReader(ReadOnlySpan<T> span) => Span = span;
}

public static class SpanReaderExtensions
{
    public static SpanReader<T> ToReader<T>(this ReadOnlySpan<T> span) => new(span);
    public static SpanReader<T> ToReader<T>(this Span<T> span) => new(span);
    public static SpanReader<T> ToReader<T>(this T[] array) => new(array);
    public static SpanReader<char> ToReader(this string? text) => new(text);

    public static ReadOnlySpan<char> ReadWord(ref this SpanReader<char> reader)
    {
        while (reader.TryMatch(' '))
            ;

        var pending = reader.Pending;
        var space = pending.IndexOf(' ');
        if (space == -1)
        {
            var result = pending;
            reader.Position += pending.Length;
            return result;
        }
        else
        {
            var result = pending.Slice(0, space);
            reader.Position += space + 1;
            return result;
        }
    }

    public static bool TryMatch<T>(
        ref this SpanReader<T> reader,
        T value
    ) where T : IEquatable<T>
    {
        if (reader.Position < reader.Span.Length && reader.Span[reader.Position].Equals(value))
        {
            ++reader.Position;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TryMatch<T>(
        ref this SpanReader<T> reader,
        ReadOnlySpan<T> values
        ) where T : IEquatable<T>
    {
        if (reader.Pending.StartsWith(values))
        {
            reader.Position += values.Length;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TryMatchBytes<T>(
        ref this SpanReader<byte> reader,
        in T value
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(
            new ReadOnlySpan<T>(in value));
        if (reader.Pending.StartsWith(bytes))
        {
            reader.Position += bytes.Length;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TryMatchBytes<T>(
        ref this SpanReader<byte> reader,
        ReadOnlySpan<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        if (reader.Pending.StartsWith(bytes))
        {
            reader.Position += bytes.Length;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TryTake<T>(
        ref this SpanReader<T> reader,
        out T? item)
    {
        var pending = reader.Pending;
        if (pending.IsEmpty)
        {
            item = default;
            return false;
        }
        else
        {
            item = pending[0];
            ++reader.Position;
            return true;
        }
    }

    public static bool TryTake<T>(
        ref this SpanReader<T> reader,
        Span<T> items)
    {
        var pending = reader.Pending;
        if (items.Length <= pending.Length)
        {
            pending.Slice(0, items.Length).CopyTo(items);
            reader.Position += items.Length;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TrySlice<T>(
        ref this SpanReader<T> reader,
        int length,
        out ReadOnlySpan<T> result)
    {
        var pending = reader.Pending;
        if (length < 0 || pending.Length < length)
        {
            result = default;
            return false;
        }
        else
        {
            reader.Position += length;
            result = pending[..length];
            return true;
        }
    }

    public static T Take<T>(ref this SpanReader<T> reader)
    {
        var result = reader.Span[reader.Position];
        ++reader.Position;
        return result;
    }

    public static ref SpanReader<T> Take<T>(ref this SpanReader<T> reader, Span<T> items)
    {
        reader.Span.Slice(reader.Position, items.Length).CopyTo(items);
        reader.Position += items.Length;
        return ref reader;
    }

    public static T Blit<T>(ref this SpanReader<byte> reader) where T : unmanaged
    {
        var value = MemoryMarshal.Read<T>(reader.Pending);
        reader.Position += Unsafe.SizeOf<T>();
        return value;
    }

    public static bool TryBlit<T>(
        ref this SpanReader<byte> reader,
        out T value
    ) where T : unmanaged
    {
        if (MemoryMarshal.TryRead(reader.Pending, out value))
        {
            reader.Position += Unsafe.SizeOf<T>();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TryBlit<T>(
        ref this SpanReader<byte> reader,
        Span<T> values
    ) where T : unmanaged
    {
        var bytes = MemoryMarshal.AsBytes(values);
        if (bytes.Length <= reader.RemainingCount)
        {
            reader.Pending.Slice(0, bytes.Length).CopyTo(bytes);
            reader.Position += bytes.Length;
            return true;
        }
        else
        {
            return false;
        }
    }
}
