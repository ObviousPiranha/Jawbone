using System;

namespace Piranha.Jawbone.Net;

public static class UdpSocket
{
    public static int Receive<TAddress>(
        this IUdpSocket<TAddress> socket,
        Span<byte> buffer,
        out Endpoint<TAddress> origin
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return socket.Receive(buffer, out origin, TimeSpan.Zero);
    }
}
