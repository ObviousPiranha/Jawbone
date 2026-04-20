using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Jawbone;

public readonly struct Utf8String : IEquatable<Utf8String>, IUtf8SpanFormattable, ISpanFormattable
{
    private readonly ImmutableArray<byte> _bytes;
    public ImmutableArray<byte> Bytes => _bytes.IsDefault ? [] : _bytes;

    public Utf8String(params ReadOnlySpan<char> chars)
    {
        var length = Encoding.UTF8.GetByteCount(chars);
        if (0 < length)
        {
            var bytes = new byte[length];
            Encoding.UTF8.GetBytes(chars, bytes);
            _bytes = ImmutableCollectionsMarshal.AsImmutableArray(bytes);
        }
    }

    public Utf8String(ImmutableArray<byte> bytes) => _bytes = bytes;
    public Utf8String(params ReadOnlySpan<byte> bytes) => ImmutableArray.Create(bytes);

    public bool Equals(Utf8String other) => _bytes.AsSpan().SequenceEqual(other._bytes.AsSpan());
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Utf8String other && Equals(other);
    public override int GetHashCode() => Utf8.GetHashCode(_bytes.AsSpan());
    public override string ToString() => _bytes.IsDefaultOrEmpty ? "" : Encoding.UTF8.GetString(_bytes.AsSpan());

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        var span = _bytes.AsSpan();
        if (utf8Destination.Length < span.Length)
        {
            span[..utf8Destination.Length].CopyTo(utf8Destination);
            bytesWritten = utf8Destination.Length;
            return false;
        }
        else
        {
            span.CopyTo(utf8Destination);
            bytesWritten = span.Length;
            return true;
        }
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        var span = _bytes.AsSpan();
        var result = Encoding.UTF8.TryGetChars(span, destination, out charsWritten);
        return result;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public static implicit operator ReadOnlyUtf8Span(Utf8String utf8String) => new(utf8String._bytes.AsSpan());
    public static implicit operator ReadOnlySpan<byte>(Utf8String utf8String) => utf8String._bytes.AsSpan();
    public static bool operator ==(Utf8String a, Utf8String b) => a.Equals(b);
    public static bool operator !=(Utf8String a, Utf8String b) => !a.Equals(b);
}