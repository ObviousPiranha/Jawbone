using System;
using System.Text;

namespace Piranha.Jawbone;

public readonly ref struct Utf8Span
{
    public readonly Span<byte> Bytes;

    public Utf8Span(Span<byte> bytes) => Bytes = bytes;
    public readonly override string ToString() => Encoding.UTF8.GetString(Bytes);

    public Utf8Enumerator GetEnumerator() => new(Bytes);

    public static implicit operator Span<byte>(Utf8Span span) => span.Bytes;
    public static implicit operator ReadOnlySpan<byte>(Utf8Span span) => span.Bytes;
    public static implicit operator Utf8Span(Span<byte> bytes) => new(bytes);
    public static implicit operator Utf8Span(Memory<byte> memory) => new(memory.Span);
    public static implicit operator Utf8Span(byte[] bytes) => new(bytes);
}
