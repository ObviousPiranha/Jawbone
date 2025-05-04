using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

static class Tcp
{
    public const int NoDelay = 1;

    public static void SetNoDelay(nuint fd)
    {
        var result = Sys.SetSockOpt(
            fd,
            IpProto.Tcp,
            NoDelay,
            1,
            Unsafe.SizeOf<int>());

        if (result == -1)
            Sys.Throw("Unable to enable TCP_NODELAY.");
    }
}
