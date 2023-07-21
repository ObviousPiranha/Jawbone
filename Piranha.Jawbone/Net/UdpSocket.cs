using System;

namespace Piranha.Jawbone.Net;

public static class UdpSocket
{
    internal const int Reuse = 1 << 0;
    internal const int Bind = 1 << 1;
    internal const int IPv6Only = 1 << 2;

    public static int Receive<TAddress>(
        this IUdpSocket<TAddress> socket,
        Span<byte> buffer,
        out Endpoint<TAddress> origin
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return socket.Receive(buffer, out origin, TimeSpan.Zero);
    }
}
