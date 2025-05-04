using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

public static class So
{
    public const int ReuseAddr = 4;

    public static void SetReuseAddr(nuint fd)
    {
        return;
        var result = Sys.SetSockOpt(
            fd,
            Sol.Socket,
            ReuseAddr,
            1,
            Unsafe.SizeOf<uint>());

        if (result == -1)
            Sys.Throw("Unable to enable SO_REUSEADDR.");
    }
}
