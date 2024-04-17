using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone;

public struct Pointer<T> where T : unmanaged
{
    public nint Address;

    public unsafe readonly ref T Dereference() => ref Unsafe.AsRef<T>(Address.ToPointer());
    public unsafe readonly Span<T> CreateSpan(int length) => new(Address.ToPointer(), length);
    public unsafe readonly ReadOnlySpan<T> CreateReadOnlySpan(int length) => new(Address.ToPointer(), length);
}
