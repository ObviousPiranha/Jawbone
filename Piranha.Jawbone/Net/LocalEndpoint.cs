using System;
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

    public readonly bool Equals(LocalEndpoint other) => NetworkOrderPort == other.NetworkOrderPort;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is LocalEndpoint other && Equals(other);
    public override readonly int GetHashCode() => NetworkOrderPort.GetHashCode();
    public override readonly string ToString() => Port.ToString();

    public static bool operator ==(LocalEndpoint a, LocalEndpoint b) => a.Equals(b);
    public static bool operator !=(LocalEndpoint a, LocalEndpoint b) => !a.Equals(b);
}
