using System;

namespace Piranha.Jawbone.Net;

public interface IUdpSocket : IDisposable
{
    void Shutdown();
}

public interface IUdpSocket<T> : IUdpSocket where T : unmanaged, IAddress<T>
{
    int Send(ReadOnlySpan<byte> message, Endpoint<T> destination);
    int Receive(Span<byte> buffer, out Endpoint<T> origin);
    Endpoint<T> GetEndpoint();
}
