using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Address32 : IAddress<Address32>
{
    public static readonly Address32 Any = default(Address32);
    public static readonly Address32 Local = new Address32(127, 0, 0, 1);
    public static readonly Address32 Broadcast = new Address32(255, 255, 255, 255);

    private readonly uint _rawAddress;

    public readonly bool IsDefault => _rawAddress == 0;
    internal readonly uint RawAddress => _rawAddress;

    public Address32(ReadOnlySpan<byte> values) : this()
    {
        var span = GetBytes(ref this);
        values.Slice(0, Math.Min(values.Length, span.Length)).CopyTo(span);
    }

    public Address32(byte a, byte b, byte c, byte d) : this()
    {
        var bytes = GetBytes(ref this);
        bytes[3] = d;
        bytes[2] = c;
        bytes[1] = b;
        bytes[0] = a;
    }

    internal Address32(uint rawAddress) => _rawAddress = rawAddress;

    public bool Equals(Address32 other) => _rawAddress == other._rawAddress;
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Address32 other && Equals(other);
    public override int GetHashCode() => _rawAddress.GetHashCode();
    public override string? ToString()
    {
        var builder = new StringBuilder(15);
        AppendTo(builder);
        return builder.ToString();
    }

    public void AppendTo(StringBuilder builder)
    {
        var span = GetReadOnlyBytes(this);
        builder
            .Append(span[0])
            .Append('.')
            .Append(span[1])
            .Append('.')
            .Append(span[2])
            .Append('.')
            .Append(span[3]);
    }

    public static Span<byte> GetBytes(ref Address32 address) => Address.GetSpanU8(ref address);
    public static ReadOnlySpan<byte> GetReadOnlyBytes(in Address32 address) => Address.GetReadOnlySpanU8(address);

    public static bool operator ==(Address32 a, Address32 b) => a.Equals(b);
    public static bool operator !=(Address32 a, Address32 b) => !a.Equals(b);
}

