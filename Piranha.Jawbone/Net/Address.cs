using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net;

public static class Address
{
    public static AnyAddress Any => default;
    public static LocalAddress Local => default;

    public static Endpoint<TAddress> OnPort<TAddress>(
        this TAddress address,
        int port
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, port);
    }

    public static LinkLocalAddress128 WithScopeId(
        this Address128 address,
        uint scopeId)
    {
        return new LinkLocalAddress128(address, scopeId);
    }

    internal static void Swap<T>(ref T a, ref T b) where T : unmanaged
    {
        var c = a;
        a = b;
        b = c;
    }
}
