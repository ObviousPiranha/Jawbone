using System;

namespace Piranha.Jawbone.Net;

public interface ITcpClient<TAddress> : IDisposable
    where TAddress : unmanaged, IAddress<TAddress>
{
    InterruptHandling HandleInterruptOnSend { get; set; }
    InterruptHandling HandleInterruptOnReceive { get; set; }
    bool HungUp { get; }

    Endpoint<TAddress> Origin { get; }
    TransferResult Send(ReadOnlySpan<byte> message);
    TransferResult Receive(Span<byte> buffer, TimeSpan timeout);
    Endpoint<TAddress> GetSocketName();
}
