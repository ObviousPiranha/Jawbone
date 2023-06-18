using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net;

public static class Address
{
    public static AnyAddress Any => default;
    public static LocalAddress Local => default;

    internal static Span<byte> GetSpanU8<T>(ref T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new Span<byte>(a, Unsafe.SizeOf<T>());
        }
    }

    internal static ReadOnlySpan<byte> GetReadOnlySpanU8<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new ReadOnlySpan<byte>(a, Unsafe.SizeOf<T>());
        }
    }

    internal static ReadOnlySpan<ushort> GetReadOnlySpanU16<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new ReadOnlySpan<ushort>(a, Unsafe.SizeOf<T>() / Unsafe.SizeOf<ushort>());
        }
    }
}
