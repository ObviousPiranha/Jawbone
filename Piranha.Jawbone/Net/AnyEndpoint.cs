using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net;

public readonly struct AnyEndpoint : IEquatable<AnyEndpoint>
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

    public bool Equals(AnyEndpoint other) => NetworkOrderPort == other.NetworkOrderPort;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AnyEndpoint other && Equals(other);
    public override int GetHashCode() => NetworkOrderPort.GetHashCode();
    public override string? ToString() => Port.ToString();

    public static implicit operator Endpoint<Address32>(AnyEndpoint anyEndpoint)
    {
        return new()
        {
            Address = Address32.Any,
            NetworkOrderPort = anyEndpoint.NetworkOrderPort
        };
    }

    public static implicit operator Endpoint<Address128>(AnyEndpoint anyEndpoint)
    {
        return new()
        {
            Address = Address128.Any,
            NetworkOrderPort = anyEndpoint.NetworkOrderPort
        };
    }
}
