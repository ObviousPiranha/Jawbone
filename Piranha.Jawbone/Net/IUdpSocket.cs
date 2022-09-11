using System;

namespace Piranha.Jawbone.Net;

public interface IUdpSocket : IDisposable
{
    void Shutdown();
}

public interface IUdpSocket<TAddress> : IUdpSocket
    where TAddress : unmanaged, IAddress<TAddress>
{
    int Send(
        ReadOnlySpan<byte> message,
        Endpoint<TAddress> destination);
    int Receive(
        Span<byte> buffer,
        out Endpoint<TAddress> origin,
        TimeSpan timeout);
    Endpoint<TAddress> GetEndpoint();
}
