using System;

namespace Piranha.Jawbone.Net;

public interface IUdpSocket<TAddress> : IDisposable
    where TAddress : unmanaged, IAddress<TAddress>
{
    int Send(
        ReadOnlySpan<byte> message,
        Endpoint<TAddress> destination);
    void Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out UdpReceiveResult<Endpoint<TAddress>> result);
    Endpoint<TAddress> GetSocketName();
}
