using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Jawbone;

public readonly ref struct ReadOnlyUtf8Span : IEquatable<ReadOnlyUtf8Span>
{
    public ReadOnlySpan<byte> Bytes { get; }

    public ReadOnlyUtf8Span(ReadOnlySpan<byte> bytes) => Bytes = bytes;
    public bool Equals(ReadOnlyUtf8Span other) => Bytes.SequenceEqual(other.Bytes);
    public override bool Equals([NotNullWhen(true)] object? obj) => false;
    public override int GetHashCode() => Utf8.GetHashCode(Bytes);
    public override string ToString() => Encoding.UTF8.GetString(Bytes);

    public Utf8Enumerator GetEnumerator() => new(Bytes);

    public static implicit operator ReadOnlySpan<byte>(ReadOnlyUtf8Span span) => span.Bytes;
    public static implicit operator ReadOnlyUtf8Span(Span<byte> bytes) => new(bytes);
    public static implicit operator ReadOnlyUtf8Span(ReadOnlySpan<byte> bytes) => new(bytes);
    public static implicit operator ReadOnlyUtf8Span(Utf8Span span) => new(span.Bytes);
    public static implicit operator ReadOnlyUtf8Span(Memory<byte> memory) => new(memory.Span);
    public static implicit operator ReadOnlyUtf8Span(ReadOnlyMemory<byte> memory) => new(memory.Span);
    public static implicit operator ReadOnlyUtf8Span(byte[] bytes) => new(bytes);

    public static bool operator ==(ReadOnlyUtf8Span a, ReadOnlyUtf8Span b) => a.Equals(b);
    public static bool operator !=(ReadOnlyUtf8Span a, ReadOnlyUtf8Span b) => !a.Equals(b);
}
