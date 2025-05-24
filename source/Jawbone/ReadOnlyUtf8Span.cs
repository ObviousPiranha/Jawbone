using System;
using System.Text;

namespace Jawbone;

public readonly ref struct ReadOnlyUtf8Span
{
    public readonly ReadOnlySpan<byte> Bytes;

    public ReadOnlyUtf8Span(ReadOnlySpan<byte> bytes) => Bytes = bytes;
    public readonly override string ToString() => Encoding.UTF8.GetString(Bytes);

    public Utf8Enumerator GetEnumerator() => new(Bytes);

    public static implicit operator ReadOnlySpan<byte>(ReadOnlyUtf8Span span) => span.Bytes;
    public static implicit operator ReadOnlyUtf8Span(Span<byte> bytes) => new(bytes);
    public static implicit operator ReadOnlyUtf8Span(ReadOnlySpan<byte> bytes) => new(bytes);
    public static implicit operator ReadOnlyUtf8Span(Utf8Span span) => new(span.Bytes);
    public static implicit operator ReadOnlyUtf8Span(Memory<byte> memory) => new(memory.Span);
    public static implicit operator ReadOnlyUtf8Span(ReadOnlyMemory<byte> memory) => new(memory.Span);
    public static implicit operator ReadOnlyUtf8Span(byte[] bytes) => new(bytes);
}
