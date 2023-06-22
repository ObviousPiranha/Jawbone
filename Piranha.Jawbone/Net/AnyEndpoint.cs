using System;
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

    public static bool operator ==(AnyEndpoint a, AnyEndpoint b) => a.Equals(b);
    public static bool operator !=(AnyEndpoint a, AnyEndpoint b) => !a.Equals(b);
}
