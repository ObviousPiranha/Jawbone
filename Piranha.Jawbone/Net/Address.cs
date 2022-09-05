using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net;

static class Address
{
    public static Span<byte> GetSpanU8<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new Span<byte>(a, Unsafe.SizeOf<T>());
        }
    }

    public static Span<ushort> GetSpanU16<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new Span<ushort>(a, Unsafe.SizeOf<T>() / Unsafe.SizeOf<ushort>());
        }
    }

    public static ReadOnlySpan<byte> GetReadOnlySpanU8<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new ReadOnlySpan<byte>(a, Unsafe.SizeOf<T>());
        }
    }
    
    public static ReadOnlySpan<ushort> GetReadOnlySpanU16<T>(in T item) where T : unmanaged
    {
        unsafe
        {
            fixed (void* a = &item)
                return new ReadOnlySpan<ushort>(a, Unsafe.SizeOf<T>() / Unsafe.SizeOf<ushort>());
        }
    }
}