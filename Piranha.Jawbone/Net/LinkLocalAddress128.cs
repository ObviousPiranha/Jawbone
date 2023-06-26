using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://stackoverflow.com/a/14083657

[StructLayout(LayoutKind.Sequential)]
public readonly struct LinkLocalAddress128 : IEquatable<LinkLocalAddress128>
{
    public readonly Address128 Address { get; init; }
    public readonly uint ScopeId { get; init; }

    public LinkLocalAddress128(Address128 address, uint scopeId)
    {
        Address = address;
        ScopeId = scopeId;
    }

    public readonly bool Equals(LinkLocalAddress128 other) => Address.Equals(other.Address) && ScopeId == other.ScopeId;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is LinkLocalAddress128 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Address, ScopeId);
    public override readonly string ToString()
    {
        var builder = new StringBuilder();
        Address.AppendTo(builder);
        --builder.Length;
        builder.Append('%').Append(ScopeId).Append(']');
        return builder.ToString();
    }

    public static bool operator ==(LinkLocalAddress128 a, LinkLocalAddress128 b) => a.Equals(b);
    public static bool operator !=(LinkLocalAddress128 a, LinkLocalAddress128 b) => !a.Equals(b);
}