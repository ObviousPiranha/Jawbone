using System;

namespace Piranha.Jawbone.Net;

static class Core
{
    public static int GetMilliseconds(TimeSpan timeSpan)
    {
        var ms64 = timeSpan.Ticks / TimeSpan.TicksPerMillisecond;
        var clamped = long.Clamp(ms64, 0, int.MaxValue);
        return (int)clamped;
    }
}
