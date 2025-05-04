using System;

namespace Piranha.Jawbone.Net;

public static class TcpListenerV4
{
    public static ITcpListener<AddressV4> Listen(Endpoint<AddressV4> bindEndpoint, int backlog)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(backlog);
        if (OperatingSystem.IsWindows())
        {
            return Windows.WindowsTcpListenerV4.Listen(bindEndpoint, backlog);
        }
        else if (OperatingSystem.IsMacOS())
        {
            return Mac.MacTcpListenerV4.Listen(bindEndpoint, backlog);
        }
        else if (OperatingSystem.IsLinux())
        {
            return Linux.LinuxTcpListenerV4.Listen(bindEndpoint, backlog);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
