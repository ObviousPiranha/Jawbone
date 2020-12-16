using System;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Point32 : IEquatable<Point32>
    {
        public static int RowMajorComparison(Point32 a, Point32 b)
        {
            return a.Y == b.Y ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y);
        }

        public static bool RowMajorLessThan(Point32 a, Point32 b)
        {
            return a.Y < b.Y || (a.Y == b.Y && a.X < b.X);
        }

        public static readonly Point32 Zero = default;

        public readonly int X;
        public readonly int Y;

        public Point32(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point32 Add(int n) => new Point32(X + n, Y + n);
        public Point32 WithX(int x) => new Point32(x, Y);
        public Point32 WithY(int y) => new Point32(X, y);
        
        public bool Equals(Point32 other) => this == other;
        public override bool Equals(object? obj) => obj is Point32 p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"({X}, {Y})";

        public static Point32 operator -(Point32 p)
        {
            return new Point32(-p.X, -p.Y);
        }
        
        public static Point32 operator +(Point32 a, Point32 b)
        {
            return new Point32(a.X + b.X, a.Y + b.Y);
        }

        public static Point32 operator -(Point32 a, Point32 b)
        {
            return new Point32(a.X - b.X, a.Y - b.Y);
        }

        public static bool operator ==(Point32 a, Point32 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point32 a, Point32 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static implicit operator Point64(Point32 p) => new Point64(p.X, p.Y);
    }
}
