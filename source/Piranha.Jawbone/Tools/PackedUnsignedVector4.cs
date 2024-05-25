using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PackedUnsignedVector4 : IEquatable<PackedUnsignedVector4>
{
    private const uint Mask10 = 0b0011_1111_1111;
    private const uint Mask2 = 0b0011;

    private readonly uint _data;

    public uint X => _data & Mask10;
    public uint Y => (_data >> 10) & Mask10;
    public uint Z => (_data >> 20) & Mask10;
    public uint W => (_data >> 30) & Mask2;

    public PackedUnsignedVector4(uint x, uint y, uint z, uint w)
    {
        _data =
            ((w & Mask2) << 30) |
            ((z & Mask10) << 20) |
            ((y & Mask10) << 10) |
            (x & Mask10);
    }

    public readonly bool Equals(PackedUnsignedVector4 other) => _data == other._data;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PackedUnsignedVector4 other && Equals(other);
    public override readonly int GetHashCode() => _data.GetHashCode();
    public override readonly string ToString() => $"{X}, ${Y}, ${Z}, ${W}";
}
