using System;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Point64 : IEquatable<Point64>
    {
        public readonly long X;
        public readonly long Y;

        public Point64(long x, long y)
        {
            X = x;
            Y = y;
        }
        
        public bool Equals(Point64 other) => X == other.X && Y == other.Y;
        public override bool Equals(object? obj) => obj is Point64 p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"({X}, {Y})";

        public static Point64 operator -(Point64 p) => new Point64(-p.X, -p.Y);
        public static Point64 operator +(Point64 a, Point64 b) => new Point64(a.X + b.X, a.Y + b.Y);
        public static Point64 operator -(Point64 a, Point64 b) => new Point64(a.X - b.X, a.Y - b.Y);
        public static bool operator ==(Point64 a, Point64 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point64 a, Point64 b) => a.X != b.X || a.Y != b.Y;
        public static explicit operator Point32(Point64 p) => new Point32((int)p.X, (int)p.Y);
    }
}
