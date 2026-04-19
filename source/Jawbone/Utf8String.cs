using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Jawbone;

public readonly struct Utf8String : IEquatable<Utf8String>
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
}