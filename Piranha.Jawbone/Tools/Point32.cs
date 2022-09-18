using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Tools;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Point32 : IEquatable<Point32>
{
    public readonly int X;
    public readonly int Y;

    public Point32(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public bool Equals(Point32 other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Point32 p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";

    public static Point32 operator -(Point32 p) => new(-p.X, -p.Y);
    public static Point32 operator +(Point32 a, Point32 b) => new(a.X + b.X, a.Y + b.Y);
    public static Point32 operator -(Point32 a, Point32 b) => new(a.X - b.X, a.Y - b.Y);
    public static Point32 operator *(Point32 a, int b) => new(a.X * b, a.Y * b);
    public static Point32 operator /(Point32 a, int b) => new(a.X / b, a.Y / b);
    public static bool operator ==(Point32 a, Point32 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Point32 a, Point32 b) => a.X != b.X || a.Y != b.Y;
    public static implicit operator Point64(Point32 p) => new(p.X, p.Y);

    public static int CrossProduct(Point32 a, Point32 b) => a.X * b.Y - b.X * a.Y;
}
