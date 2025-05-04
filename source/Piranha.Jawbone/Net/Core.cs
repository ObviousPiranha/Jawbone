using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net;

static class Core
{
    public static int GetMilliseconds(TimeSpan timeSpan)
    {
        var ms64 = timeSpan.Ticks / TimeSpan.TicksPerMillisecond;
        var clamped = long.Clamp(ms64, 0, int.MaxValue);
        return (int)clamped;
    }

    [DoesNotReturn]
    public static void ThrowWrongAddressFamily()
    {
        throw new InvalidOperationException("Incorrect address family.");
    }
}
