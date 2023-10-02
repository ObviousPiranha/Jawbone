using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public readonly ref struct Arena
{
    public readonly Span<byte> Bytes { get; }

    public Arena(int size)
    {
        Bytes = GC.AllocateUninitializedArray<byte>(size, true);
    }
    
    private Arena(Span<byte> bytes) => Bytes = bytes;

    public Arena Advanced(int count) => new(Bytes.Slice(count));

    public unsafe Arena AllocateUninitialized<T>(int count, out Span<T> allocated) where T : unmanaged
    {
        nint p;
        fixed (byte* b = Bytes)
            p = new(b);
        
        var alignment = Alignment.Of<T>();
        var padding = (int)(-p & (alignment - 1));
        var arraySize = count * Unsafe.SizeOf<T>();

        if (Bytes.Length < padding + arraySize)
            throw new InvalidOperationException("Not enough memory left in arena.");
        
        allocated = MemoryMarshal.Cast<byte, T>(Bytes.Slice(padding, arraySize));
        return Advanced(padding + arraySize);
    }

    public Arena Allocate<T>(int count, out Span<T> allocated) where T : unmanaged
    {
        var result = AllocateUninitialized(count, out allocated);
        allocated.Clear();
        return result;
    }
}