using System;

namespace Piranha.Jawbone.Net;

public static class UdpClientV6
{
    public static IUdpClient<AddressV6> Connect(Endpoint<AddressV6> endpoint)
    {
        if (OperatingSystem.IsLinux())
        {
            return Linux.LinuxUdpClientV6.Connect(endpoint);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
