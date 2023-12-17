using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net;

[StructLayout(LayoutKind.Sequential)]
public readonly struct NetworkPort : IEquatable<NetworkPort>
{
    public readonly ushort NetworkValue { get; init; }

    public readonly int HostValue
    {
        get
        {
            return BitConverter.IsLittleEndian ?
                BinaryPrimitives.ReverseEndianness(NetworkValue) :
                NetworkValue;
        }

        init
        {
            var port = checked((ushort)value);
            NetworkValue = BitConverter.IsLittleEndian ?
                BinaryPrimitives.ReverseEndianness(port) :
                port;
        }
    }

    public readonly bool Equals(NetworkPort other) => NetworkValue == other.NetworkValue;
    public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is NetworkPort other && Equals(other);
    public readonly override int GetHashCode() => NetworkValue.GetHashCode();
    public readonly override string ToString() => HostValue.ToString();

    public static bool operator ==(NetworkPort a, NetworkPort b) => a.Equals(b);
    public static bool operator !=(NetworkPort a, NetworkPort b) => !a.Equals(b);
}
