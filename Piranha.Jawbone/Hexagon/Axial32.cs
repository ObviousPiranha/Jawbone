using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Hexagon;

// https://www.redblobgames.com/grids/hexagons/

[StructLayout(LayoutKind.Sequential)]
public readonly struct Axial32 : IEquatable<Axial32>
{
    public readonly int Q { get; }
    public readonly int R { get; }
    public readonly int S => -Q - R;

    public Axial32(int q, int r)
    {
        Q = q;
        R = r;
    }

    public bool Equals(Axial32 other) => Q == other.Q && R == other.R;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Axial32 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Q, R);
    public override string? ToString() => $"{Q}, {R}, {S}";

    public static bool operator ==(Axial32 a, Axial32 b) => a.Equals(b);
    public static bool operator !=(Axial32 a, Axial32 b) => !a.Equals(b);
}
