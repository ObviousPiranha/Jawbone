using System;

namespace Piranha.Jawbone.Net;

public static class TcpListenerV6
{
    public static ITcpListener<AddressV6> Listen(Endpoint<AddressV6> bindEndpoint, int backlog)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(backlog);
        if (OperatingSystem.IsWindows())
            return Windows.WindowsTcpListenerV6.Listen(bindEndpoint, backlog);
        if (OperatingSystem.IsMacOS())
            return Mac.MacTcpListenerV6.Listen(bindEndpoint, backlog);
        if (OperatingSystem.IsLinux())
            return Linux.LinuxTcpListenerV6.Listen(bindEndpoint, backlog);
        throw new PlatformNotSupportedException();
    }
}
