using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

public static class Endpoint
{
    public static Endpoint<TAddress> Create<TAddress>(TAddress address, int port)
        where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, port);
    }

    public static Endpoint<TAddress> Create<TAddress>(TAddress address, NetworkPort port)
        where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, port);
    }

    public static Endpoint<AddressV6> MapToV6(this Endpoint<AddressV4> endpoint)
    {
        return Create((AddressV6)endpoint.Address, endpoint.Port);
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct Endpoint<TAddress> : IEquatable<Endpoint<TAddress>>
    where TAddress : unmanaged, IAddress<TAddress>
{
    public readonly TAddress Address { get; init; }
    public readonly NetworkPort Port { get; init; }
    public readonly bool IsDefault => Address.IsDefault && Port.NetworkValue == 0;

    public Endpoint(TAddress address, NetworkPort port)
    {
        Address = address;
        Port = port;
    }

    public Endpoint(TAddress address, int port) : this(address, (NetworkPort)port)
    {
    }

    public bool Equals(Endpoint<TAddress> other) => Address.Equals(other.Address) && Port.Equals(other.Port);
    public override bool Equals(object? obj) => obj is Endpoint<TAddress> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Address, Port);
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendEndpoint(this);
        return builder.ToString();
    }

    public static bool operator ==(Endpoint<TAddress> a, Endpoint<TAddress> b) => a.Equals(b);
    public static bool operator !=(Endpoint<TAddress> a, Endpoint<TAddress> b) => !a.Equals(b);
}
