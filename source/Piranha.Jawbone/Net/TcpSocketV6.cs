using System;

namespace Piranha.Jawbone.Net;

public static class TcpSocketV6
{
    public static ITcpSocket<AddressV6> Connect(Endpoint<AddressV6> endpoint)
    {
        if (OperatingSystem.IsLinux())
        {
            return Linux.LinuxTcpSocketV6.Connect(endpoint);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
