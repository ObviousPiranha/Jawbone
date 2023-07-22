using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://stackoverflow.com/a/14083657

[StructLayout(LayoutKind.Sequential)]
public readonly struct Address128WithScopeId : IAddress<Address128WithScopeId>
{
    public readonly Address128 Address { get; init; }
    public readonly uint ScopeId { get; init; }

    public bool IsDefault => Address.IsDefault && ScopeId == 0;
    public bool IsLinkLocal => Address.IsLinkLocal;
    public bool IsLoopback => Address.IsLoopback;

    public static Address128WithScopeId Any => new(Address128.Any, 0);
    public static Address128WithScopeId Local => new(Address128.Local, 0);

    public Address128WithScopeId(Address128 address, uint scopeId)
    {
        Address = address;
        ScopeId = scopeId;
    }

    public readonly bool Equals(Address128WithScopeId other) => Address.Equals(other.Address) && ScopeId == other.ScopeId;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Address128WithScopeId other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Address, ScopeId);
    public override readonly string ToString()
    {
        var builder = new StringBuilder();
        Address.AppendTo(builder);
        --builder.Length;
        builder.Append('%').Append(ScopeId).Append(']');
        return builder.ToString();
    }

    public void AppendTo(StringBuilder builder)
    {
        throw new NotImplementedException();
    }

    public static Address128WithScopeId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Address128WithScopeId result)
    {
        throw new NotImplementedException();
    }

    public static Address128WithScopeId Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Address128WithScopeId result)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(Address128WithScopeId a, Address128WithScopeId b) => a.Equals(b);
    public static bool operator !=(Address128WithScopeId a, Address128WithScopeId b) => !a.Equals(b);

    public static implicit operator Address128WithScopeId(Address128 address) => new(address, 0);
    public static implicit operator Address128WithScopeId(AnyAddress anyAddress) => Address128.Any;
    public static implicit operator Address128WithScopeId(LocalAddress localAddress) => Address128.Local;
}
