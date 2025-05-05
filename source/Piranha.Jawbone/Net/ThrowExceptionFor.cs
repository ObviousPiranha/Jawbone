using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net;

static class ThrowExceptionFor
{
    [DoesNotReturn]
    public static void WrongAddressFamily()
    {
        throw new InvalidOperationException("Incorrect address family.");
    }

    [DoesNotReturn]
    public static void WrongAddressLength()
    {
        throw new InvalidOperationException("Address length does not match.");
    }
}
