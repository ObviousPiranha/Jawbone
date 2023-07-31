using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Point64 : IEquatable<Point64>
{
    public long X;
    public long Y;

    public Point64(long x, long y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Point64 other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Point64 p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";

    public static Point64 operator -(Point64 p) => new(-p.X, -p.Y);
    public static Point64 operator +(Point64 a, Point64 b) => new(a.X + b.X, a.Y + b.Y);
    public static Point64 operator -(Point64 a, Point64 b) => new(a.X - b.X, a.Y - b.Y);
    public static bool operator ==(Point64 a, Point64 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Point64 a, Point64 b) => a.X != b.X || a.Y != b.Y;
    public static explicit operator Point32(Point64 p) => new((int)p.X, (int)p.Y);
}
