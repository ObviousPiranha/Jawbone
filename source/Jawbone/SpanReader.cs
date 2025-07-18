using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone;

public ref struct SpanReader<T>
{
    public readonly ReadOnlySpan<T> Span;
    public int Position;

    public readonly ReadOnlySpan<T> Pending => Span.Slice(Position);
    public readonly ReadOnlySpan<T> Consumed => Span.Slice(0, Position);
    public readonly int RemainingCount => Span.Length - Position;
    public readonly bool WasConsumed => Span.Length <= Position;

    public SpanReader(ReadOnlySpan<T> span) => Span = span;
}

public static class SpanReader
{
    public static SpanReader<T> Create<T>(ReadOnlySpan<T> span) => new(span);
    public static SpanReader<T> Create<T>(Span<T> span) => new(span);
    public static SpanReader<T> Create<T>(ReadOnlyMemory<T> memory) => new(memory.Span);
    public static SpanReader<T> Create<T>(Memory<T> memory) => new(memory.Span);
    public static SpanReader<T> Create<T>(T[]? array) => new(array);
    public static SpanReader<T> Create<T>(ArraySegment<T> segment) => new(segment);
    public static SpanReader<T> Create<T>(List<T>? list) => new(CollectionsMarshal.AsSpan(list));
    public static SpanReader<char> Create(string? text) => new(text);

    public static ReadOnlySpan<char> ReadWord(ref this SpanReader<char> reader)
    {
        reader.SkipAll(' ');
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

    public static void SkipAll<T>(
        ref this SpanReader<T> reader,
        T item
    ) where T : IEquatable<T>
    {
        var index = reader.Pending.IndexOfAnyExcept(item);
        if (index == -1)
            reader.Position = reader.Span.Length;
        else
            reader.Position += index;
    }

    public static void SkipAll<T>(
        ref this SpanReader<T> reader,
        ReadOnlySpan<T> items
    ) where T : IEquatable<T>
    {
        var index = reader.Pending.IndexOfAnyExcept(items);
        if (index == -1)
            reader.Position = reader.Span.Length;
        else
            reader.Position += index;
    }

    public static void SkipAll<T>(
        ref this SpanReader<T> reader,
        SearchValues<T> items
    ) where T : IEquatable<T>
    {
        var index = reader.Pending.IndexOfAnyExcept(items);
        if (index == -1)
            reader.Position = reader.Span.Length;
        else
            reader.Position += index;
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

    public static void Take<T>(ref this SpanReader<T> reader, Span<T> items)
    {
        reader.Span.Slice(reader.Position, items.Length).CopyTo(items);
        reader.Position += items.Length;
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

    public static bool TryReadBigEndianInt32(
        ref this SpanReader<byte> reader,
        out int result)
    {
        if (reader.TryBlit<int>(out var bigEndianValue))
        {
            result = BitConverter.IsLittleEndian ?
                BinaryPrimitives.ReverseEndianness(bigEndianValue) :
                bigEndianValue;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static int ReadBigEndianInt32(
        ref this SpanReader<byte> reader)
    {
        var bigEndianValue = reader.Blit<int>();
        var result = BitConverter.IsLittleEndian ?
            BinaryPrimitives.ReverseEndianness(bigEndianValue) :
            bigEndianValue;
        return result;
    }

    public static bool TryReadBigEndianUInt32(
        ref this SpanReader<byte> reader,
        out uint result)
    {
        if (reader.TryBlit(out uint bigEndianValue))
        {
            result = BitConverter.IsLittleEndian ?
                BinaryPrimitives.ReverseEndianness(bigEndianValue) :
                bigEndianValue;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static uint ReadBigEndianUInt32(
        ref this SpanReader<byte> reader)
    {
        var bigEndianValue = reader.Blit<uint>();
        var result = BitConverter.IsLittleEndian ?
            BinaryPrimitives.ReverseEndianness(bigEndianValue) :
            bigEndianValue;
        return result;
    }

    public static bool TryReadUtf32(ref this SpanReader<char> reader, out int utf32)
    {
        if (reader.Span.Length <= reader.Position)
        {
            utf32 = 0;
            return false;
        }

        if (char.IsHighSurrogate(reader.Span[reader.Position]))
        {
            if (reader.Position + 1 < reader.Span.Length &&
                char.IsLowSurrogate(reader.Span[reader.Position + 1]))
            {
                utf32 = char.ConvertToUtf32(
                    reader.Span[reader.Position],
                    reader.Span[reader.Position + 1]);
                reader.Position += 2;
                return true;
            }
            else
            {
                utf32 = 0;
                return false;
            }
        }
        else
        {
            utf32 = reader.Span[reader.Position++];
            return true;
        }
    }
}
