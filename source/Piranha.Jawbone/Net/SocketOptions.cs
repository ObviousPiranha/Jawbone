using System;

namespace Piranha.Jawbone.Net;

[Flags]
public enum SocketOptions
{
    None,
    DoNotReuseAddress = 1 << 0,
    DisableTcpNoDelay = 1 << 1,
    EnableDualMode = 1 << 2,
    ThrowOnInterrupt = 1 << 3

}
