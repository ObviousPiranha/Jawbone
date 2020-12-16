using System;

namespace Piranha.Tools
{
    public readonly struct Point64 : IEquatable<Point64>
    {
        public static readonly Point64 Zero = default;
        
        public long X { get; }
        public long Y { get; }

        public Point64(long x, long y)
        {
            X = x;
            Y = y;
        }

        public Point64 WithX(long x) => new Point64(x, Y);
        public Point64 WithY(long y) => new Point64(X, y);
        
        public override bool Equals(object? obj) => obj is Point64 p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"({X}, {Y})";
        public bool Equals(Point64 other) => X == other.X && Y == other.Y;

        public static Point64 operator -(Point64 p)
        {
            return new Point64(-p.X, -p.Y);
        }
        
        public static Point64 operator +(Point64 a, Point64 b)
        {
            return new Point64(a.X + b.X, a.Y + b.Y);
        }

        public static Point64 operator -(Point64 a, Point64 b)
        {
            return new Point64(a.X - b.X, a.Y - b.Y);
        }
    }
}
