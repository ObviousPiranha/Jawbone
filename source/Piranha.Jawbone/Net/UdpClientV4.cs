using System;

namespace Piranha.Jawbone.Net;

public static class UdpClientV4
{
    public static IUdpClient<AddressV4> Connect(Endpoint<AddressV4> endpoint)
    {
        if (OperatingSystem.IsLinux())
        {
            return Linux.LinuxUdpClientV4.Connect(endpoint);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
