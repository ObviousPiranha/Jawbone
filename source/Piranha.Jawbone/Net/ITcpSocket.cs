using System;

namespace Piranha.Jawbone.Net;

public interface ITcpSocket : IDisposable
{
    int Send(ReadOnlySpan<byte> message);
    int? Receive(Span<byte> buffer, TimeSpan timeout);
}

public interface ITcpSocket<TAddress> : ITcpSocket
    where TAddress : unmanaged, IAddress<TAddress>
{
    Endpoint<TAddress> Origin { get; }

    Endpoint<TAddress> GetSocketName();
}
