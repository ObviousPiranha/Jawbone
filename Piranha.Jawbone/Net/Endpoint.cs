using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Endpoint<TAddress> : IEquatable<Endpoint<TAddress>> where TAddress : unmanaged, IAddress<TAddress>
{
    private static void ValidatePort(int port)
    {
        if (port < 0 || ushort.MaxValue < port)
        {
            throw new ArgumentOutOfRangeException(
                nameof(port),
                $"Port must be in range from 0 to {ushort.MaxValue}.");
        }
    }

    public readonly TAddress Address { get; init; }
    public readonly ushort RawPort { get; init; }
    public readonly int Port
    {
        get => BitConverter.IsLittleEndian ? SwapBytes(RawPort) : RawPort;
        init
        {
            ValidatePort(value);
            RawPort = BitConverter.IsLittleEndian ? (ushort)SwapBytes(value) : (ushort)value;
        }
    }

    public Endpoint(TAddress address, int port) : this()
    {
        Address = address;
        Port = port;
    }

    public bool Equals(Endpoint<TAddress> other) => Address.Equals(other.Address) && RawPort == other.RawPort;
    public override bool Equals(object? obj) => obj is Endpoint<TAddress> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Address, RawPort);
    public override string? ToString()
    {
        var builder = new StringBuilder();
        builder.AppendEndpoint(this);
        return builder.ToString();
    }

    private static int SwapBytes(int n)
    {
        return ((n & 0xff) << 8) | (n >> 8);
    }

    public static bool operator ==(Endpoint<TAddress> a, Endpoint<TAddress> b) => a.Equals(b);
    public static bool operator !=(Endpoint<TAddress> a, Endpoint<TAddress> b) => !a.Equals(b);
}
