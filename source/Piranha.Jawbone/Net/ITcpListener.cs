using System;

namespace Piranha.Jawbone.Net;

public interface ITcpListener<TAddress> : IDisposable
    where TAddress : unmanaged, IAddress<TAddress>
{
    ITcpClient<TAddress>? Accept(TimeSpan timeout);
    Endpoint<TAddress> GetSocketName();
}
