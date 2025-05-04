using System;

namespace Piranha.Jawbone.Net;

public static class TcpSocketV4
{
    public static ITcpSocket<AddressV4> Connect(Endpoint<AddressV4> endpoint)
    {
        if (OperatingSystem.IsWindows())
        {
            return Windows.WindowsTcpSocketV4.Connect(endpoint);
        }
        else if (OperatingSystem.IsMacOS())
        {
            return Mac.MacTcpSocketV4.Connect(endpoint);
        }
        else if (OperatingSystem.IsLinux())
        {
            return Linux.LinuxTcpSocketV4.Connect(endpoint);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
