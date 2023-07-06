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

    internal static void SwapU16(ref ushort n)
    {
        n = unchecked((ushort)(((n & 0xff) << 8) | (n >> 8)));
    }
}
