using System;
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

    internal static void ValidatePort(int port)
    {
        if (port < 0 || ushort.MaxValue < port)
        {
            throw new ArgumentOutOfRangeException(
                nameof(port),
                $"Port must be in range from 0 to {ushort.MaxValue}.");
        }
    }

    internal static int SwapBytes(int n)
    {
        return ((n & 0xff) << 8) | (n >> 8);
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct Endpoint<TAddress> : IEquatable<Endpoint<TAddress>>
    where TAddress : unmanaged, IAddress<TAddress>
{
    public readonly TAddress Address { get; init; }
    public readonly ushort NetworkOrderPort { get; init; }
    public readonly bool IsDefault => Address.IsDefault && NetworkOrderPort == 0;
    public readonly int Port
    {
        get => BitConverter.IsLittleEndian ? Endpoint.SwapBytes(NetworkOrderPort) : NetworkOrderPort;
        init
        {
            Endpoint.ValidatePort(value);
            NetworkOrderPort = BitConverter.IsLittleEndian ? (ushort)Endpoint.SwapBytes(value) : (ushort)value;
        }
    }

    public Endpoint(TAddress address, int port) : this()
    {
        Address = address;
        Port = port;
    }

    public bool Equals(Endpoint<TAddress> other) => Address.Equals(other.Address) && NetworkOrderPort == other.NetworkOrderPort;
    public override bool Equals(object? obj) => obj is Endpoint<TAddress> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Address, NetworkOrderPort);
    public override string? ToString()
    {
        var builder = new StringBuilder();
        builder.AppendEndpoint(this);
        return builder.ToString();
    }

    public static bool operator ==(Endpoint<TAddress> a, Endpoint<TAddress> b) => a.Equals(b);
    public static bool operator !=(Endpoint<TAddress> a, Endpoint<TAddress> b) => !a.Equals(b);
}
