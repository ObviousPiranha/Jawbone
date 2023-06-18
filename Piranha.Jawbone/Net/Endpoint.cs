using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

public static class Endpoint
{
    public static AnyEndpoint Any => default;
    
    public static Endpoint<TAddress> Create<TAddress>(TAddress address, int port)
        where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, port);
    }

    public static Endpoint<Address32> Local32(int port) => new(Address32.Local, port);
    public static Endpoint<Address128> Local128(int port) => new(Address128.Local, port);

    public static Endpoint<Address32> Any32(int port) => new(Address32.Any, port);
    public static Endpoint<Address128> Any128(int port) => new(Address128.Any, port);
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct Endpoint<TAddress> : IEquatable<Endpoint<TAddress>>
    where TAddress : unmanaged, IAddress<TAddress>
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
    public readonly bool IsDefault => Address.IsDefault && RawPort == 0;
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
