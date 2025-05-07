using System;

namespace Piranha.Jawbone.Net;

public interface IUdpSocket<TAddress> : IDisposable
    where TAddress : unmanaged, IAddress<TAddress>
{
    bool ThrowOnInterruptSend { get; set; }
    bool ThrowOnInterruptReceive { get; set; }

    int Send(
        ReadOnlySpan<byte> message,
        Endpoint<TAddress> destination);
    int? Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out Endpoint<TAddress> origin);
    Endpoint<TAddress> GetSocketName();
}

public static class UdpSocketExtensions
{
    public static void Receive<TAddress>(
        this IUdpSocket<TAddress> udpSocket,
        ref Span<byte> buffer,
        TimeSpan timeout,
        out Endpoint<TAddress> origin)
        where TAddress : unmanaged, IAddress<TAddress>
    {
        var result = udpSocket.Receive(buffer, timeout, out origin);
        if (!result.HasValue)
            throw new TimeoutException();
        buffer = buffer[..result.Value];
    }

    public static void Receive<TAddress>(
        this IUdpSocket<TAddress> udpSocket,
        ref Span<byte> buffer,
        TimeSpan timeout)
        where TAddress : unmanaged, IAddress<TAddress>
    {
        var result = udpSocket.Receive(buffer, timeout, out _);
        if (!result.HasValue)
            throw new TimeoutException();
        buffer = buffer[..result.Value];
    }
}
