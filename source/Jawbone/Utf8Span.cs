using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Jawbone;

public readonly ref struct Utf8Span : IEquatable<Utf8Span>
{
    public Span<byte> Bytes { get; }

    public Utf8Span(Span<byte> bytes) => Bytes = bytes;
    public bool Equals(Utf8Span other) => Bytes.SequenceEqual(other.Bytes);
    public override bool Equals([NotNullWhen(true)] object? obj) => false;
    public override int GetHashCode() => Utf8.GetHashCode(Bytes);
    public override string ToString() => Encoding.UTF8.GetString(Bytes);

    public Utf8Enumerator GetEnumerator() => new(Bytes);

    public static implicit operator Span<byte>(Utf8Span span) => span.Bytes;
    public static implicit operator ReadOnlySpan<byte>(Utf8Span span) => span.Bytes;
    public static implicit operator Utf8Span(Span<byte> bytes) => new(bytes);
    public static implicit operator Utf8Span(Memory<byte> memory) => new(memory.Span);
    public static implicit operator Utf8Span(byte[] bytes) => new(bytes);

    public static bool operator ==(Utf8Span a, Utf8Span b) => a.Equals(b);
    public static bool operator !=(Utf8Span a, Utf8Span b) => !a.Equals(b);
}
