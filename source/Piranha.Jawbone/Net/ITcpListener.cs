using System;

namespace Piranha.Jawbone.Net;

public interface ITcpListener<TAddress> : IDisposable
    where TAddress : unmanaged, IAddress<TAddress>
{
    ITcpSocket<TAddress>? Accept(TimeSpan timeout);
}
