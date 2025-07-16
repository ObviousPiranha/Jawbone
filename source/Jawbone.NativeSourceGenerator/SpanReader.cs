using System;
using System.Text;

namespace Jawbone.NativeSourceGenerator;

ref struct SpanReader<T>
{
    public readonly ReadOnlySpan<T> Span;
    public int Position;

    public readonly ReadOnlySpan<T> Consumed => Span[..Position];
    public readonly ReadOnlySpan<T> Remaining => Span[Position..];

    public SpanReader(ReadOnlySpan<T> span) => Span = span;
}

static class SpanReader
{
    public static SpanReader<T> Create<T>(Span<T> span) => new(span);
    public static SpanReader<T> Create<T>(ReadOnlySpan<T> span) => new(span);
    public static SpanReader<T> Create<T>(T[]? array) => new(array);
    public static SpanReader<char> Create(string? s) => new(s);

    public static ReadOnlySpan<T> ReadUntil<T>(ReadOnlySpan<T> span, T item) where T : IEquatable<T>
    {
        var index = span.IndexOf(item);
        return index == -1 ? span : span[..index];
    }

    public static ReadOnlySpan<char> ReadLastWord(ReadOnlySpan<char> span)
    {
        var index = span.LastIndexOf(' ');
        var result = index == -1 ? span : span.Slice(index + 1);
        return result;
    }

    public static bool TryReadSandwich<T>(
        ReadOnlySpan<T> span,
        T left,
        T right,
        out ReadOnlySpan<T> result) where T : IEquatable<T>
    {
        result = default;
        var remaining = span;
        var leftIndex = remaining.IndexOf(left);
        if (leftIndex == -1)
            return false;
        remaining = remaining.Slice(leftIndex + 1);
        var rightIndex = remaining.IndexOf(right);
        if (rightIndex == -1)
            return false;
        result = remaining[..rightIndex];
        return true;
    }

    public static bool TryReadSandwich<T>(
        ref this SpanReader<T> reader,
        T left,
        T right,
        out ReadOnlySpan<T> result) where T : IEquatable<T>
    {
        result = default;
        var remaining = reader.Remaining;
        var leftIndex = remaining.IndexOf(left);
        if (leftIndex == -1)
            return false;
        remaining = remaining.Slice(leftIndex + 1);
        var rightIndex = remaining.IndexOf(right);
        if (rightIndex == -1)
            return false;
        result = remaining[..rightIndex];
        reader.Position += leftIndex + 1 + rightIndex + 1;
        return true;
    }

    public static bool TryReadSandwich<T>(
        ref this SpanReader<T> reader,
        ReadOnlySpan<T> left,
        ReadOnlySpan<T> right,
        out ReadOnlySpan<T> result) where T : IEquatable<T>
    {
        result = default;
        var remaining = reader.Remaining;
        var leftIndex = left.IsEmpty ? 0 : remaining.IndexOf(left);
        if (leftIndex == -1)
            return false;
        remaining = remaining.Slice(leftIndex + left.Length);
        var rightIndex = right.IsEmpty ? remaining.Length : remaining.IndexOf(right);
        if (rightIndex == -1)
            return false;
        result = remaining[..rightIndex];
        reader.Position += leftIndex + left.Length + rightIndex + right.Length;
        return true;
    }
}
