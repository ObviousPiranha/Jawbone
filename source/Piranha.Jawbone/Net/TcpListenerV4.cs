using System;

namespace Piranha.Jawbone.Net;

public static class TcpListenerV4
{
    public static ITcpListener<AddressV4> Listen(Endpoint<AddressV4> bindEndpoint, int backlog)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(backlog);
        if (OperatingSystem.IsLinux())
        {
            return Linux.LinuxTcpListenerV4.Listen(bindEndpoint, backlog);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
