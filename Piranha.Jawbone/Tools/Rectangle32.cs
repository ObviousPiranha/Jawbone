using System;
using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Rectangle32 : IEquatable<Rectangle32>
    {
        public Point32 Low { get; }
        public Point32 High { get; }

        public Point32 Size => High - Low;
        public int Width => High.X - Low.X;
        public int Height => High.Y - Low.Y;
        public int Area => Width * Height;

        public Rectangle32(Point32 low, Point32 high)
        {
            Low = low;
            High = high;
        }

        public override bool Equals(object? obj) => obj is Rectangle32 r && Equals(r);
        public override int GetHashCode() => HashCode.Combine(Low, High);
        public override string ToString() => $"{Low} to {High}";

        public bool Equals(Rectangle32 other)
        {
            return Low.Equals(other.Low) && High.Equals(other.High);
        }

        public Rectangle32 Shrunk(int n)
        {
            var offset = new Point32(n, n);
            return new Rectangle32(Low + offset, High - offset);
        }
    }
}
