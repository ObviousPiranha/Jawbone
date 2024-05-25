using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Circle32 : IEquatable<Circle32>
{
    public readonly Point32 Center;
    public readonly int Radius;

    public Circle32(Point32 center, int radius)
    {
        Center = center;
        Radius = radius;
    }

    public readonly bool Equals(Circle32 other) => Center.Equals(other.Center) && Radius == other.Radius;
    public override readonly bool Equals(object? obj) => obj is Circle32 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Center, Radius);
    public override readonly string ToString() => $"center {Center} radius {Radius}";
}

