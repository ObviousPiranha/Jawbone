namespace DotNetRemover;

ref struct SpanReader<T>
{
    public readonly ReadOnlySpan<T> Span { get; }
    public int Position;

    public readonly ReadOnlySpan<T> Consumed => Span[..Position];
    public readonly ReadOnlySpan<T> Remaining => Span[Position..];
    public readonly bool Completed => Span.Length <= Position;

    public SpanReader(ReadOnlySpan<T> span) => Span = span;
}

static class SpanReader
{
    public static SpanReader<T> Create<T>(ReadOnlySpan<T> span) => new(span);
    public static SpanReader<char> Create(string? s) => new(s);

    public static ReadOnlySpan<T> ReadUntilValueOrEnd<T>(ref this SpanReader<T> reader, T value) where T : IEquatable<T>
    {
        var remaining = reader.Remaining;
        var index = remaining.IndexOf(value);
        if (index == -1)
        {
            reader.Position = reader.Span.Length;
            return remaining;
        }
        else
        {
            reader.Position += index + 1;
            return remaining[..index];
        }
    }

    public static ReadOnlySpan<T> TrimAt<T>(ReadOnlySpan<T> span, T value) where T : IEquatable<T>
    {
        var index = span.IndexOf(value);
        var result = index == -1 ? span : span[..index];
        return result;
    }

    private static T Map<T>(this T value, T target, T replacement) where T : IEquatable<T>
    {
        return value.Equals(target) ? replacement : value;
    }
}