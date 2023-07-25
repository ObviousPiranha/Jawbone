using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

    public static Address128WithScopeId WithScopeId(
        this Address128 address,
        uint scopeId = 0)
    {
        return new Address128WithScopeId(address, scopeId);
    }

    public static Span<byte> AsBytes<TAddress>(
        ref TAddress address
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return MemoryMarshal.AsBytes(new Span<TAddress>(ref address));
    }

    public static ReadOnlySpan<byte> AsReadOnlyBytes<TAddress>(
        in TAddress address
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return MemoryMarshal.AsBytes(new ReadOnlySpan<TAddress>(address));
    }
}
