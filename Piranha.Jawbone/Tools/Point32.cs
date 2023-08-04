using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Point32 : IEquatable<Point32>
{
    public int X;
    public int Y;

    public Point32(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly bool Equals(Point32 other) => X == other.X && Y == other.Y;
    public override readonly bool Equals(object? obj) => obj is Point32 p && Equals(p);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);
    public override readonly string ToString() => $"({X}, {Y})";

    public static Point32 operator -(Point32 p) => new(-p.X, -p.Y);
    public static Point32 operator +(Point32 a, Point32 b) => new(a.X + b.X, a.Y + b.Y);
    public static Point32 operator -(Point32 a, Point32 b) => new(a.X - b.X, a.Y - b.Y);
    public static Point32 operator +(Point32 a, int b) => new(a.X + b, a.Y + b);
    public static Point32 operator -(Point32 a, int b) => new(a.X - b, a.Y - b);
    public static Point32 operator *(Point32 a, int b) => new(a.X * b, a.Y * b);
    public static Point32 operator /(Point32 a, int b) => new(a.X / b, a.Y / b);
    public static Vector2 operator *(Point32 a, float b) => new(a.X * b, a.Y * b);
    public static Vector2 operator /(Point32 a, float b) => new(a.X / b, a.Y / b);
    public static Point32 operator <<(Point32 a, int b) => new(a.X << b, a.Y << b);
    public static Point32 operator >>(Point32 a, int b) => new(a.X >> b, a.Y >> b);
    public static bool operator ==(Point32 a, Point32 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Point32 a, Point32 b) => a.X != b.X || a.Y != b.Y;
    public static implicit operator Point64(Point32 p) => new(p.X, p.Y);
    public static explicit operator Vector2(Point32 p) => new(p.X, p.Y);

    public static int CrossProduct(Point32 a, Point32 b) => a.X * b.Y - b.X * a.Y;
}
