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

    internal static void Swap<T>(ref T a, ref T b) where T : unmanaged
    {
        var c = a;
        a = b;
        b = c;
    }

    internal static Span<byte> GetSpanU8<T>(ref T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new Span<byte>(a, sizeof(T));
        }
    }

    internal static ReadOnlySpan<byte> GetReadOnlySpanU8<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new ReadOnlySpan<byte>(a, sizeof(T));
        }
    }

    internal static ReadOnlySpan<ushort> GetReadOnlySpanU16<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new ReadOnlySpan<ushort>(a, sizeof(T) / sizeof(ushort));
        }
    }
}
