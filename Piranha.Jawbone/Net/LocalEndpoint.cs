using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net;

public readonly struct LocalEndpoint : IEquatable<LocalEndpoint>
{
    public readonly ushort NetworkOrderPort { get; init; }
    public readonly int Port
    {
        get => BitConverter.IsLittleEndian ? Endpoint.SwapBytes(NetworkOrderPort) : NetworkOrderPort;
        init
        {
            Endpoint.ValidatePort(value);
            NetworkOrderPort = BitConverter.IsLittleEndian ? (ushort)Endpoint.SwapBytes(value) : (ushort)value;
        }
    }

    public bool Equals(LocalEndpoint other) => NetworkOrderPort == other.NetworkOrderPort;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is LocalEndpoint other && Equals(other);
    public override int GetHashCode() => NetworkOrderPort.GetHashCode();
    public override string? ToString() => Port.ToString();

    public static implicit operator Endpoint<Address32>(LocalEndpoint LocalEndpoint)
    {
        return new()
        {
            Address = Address32.Local,
            NetworkOrderPort = LocalEndpoint.NetworkOrderPort
        };
    }

    public static implicit operator Endpoint<Address128>(LocalEndpoint LocalEndpoint)
    {
        return new()
        {
            Address = Address128.Local,
            NetworkOrderPort = LocalEndpoint.NetworkOrderPort
        };
    }
}
