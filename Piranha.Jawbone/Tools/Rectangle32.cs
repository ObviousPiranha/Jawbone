using System;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Rectangle32 : IEquatable<Rectangle32>
    {
        public readonly Point32 Position;
        public readonly Point32 Size;

        public Rectangle32(Point32 position, Point32 size)
        {
            Position = position;
            Size = size;
        }

        public override bool Equals(object? obj) => obj is Rectangle32 r && Equals(r);
        public override int GetHashCode() => HashCode.Combine(Position.X, Position.Y, Size.X, Size.Y);
        public override string ToString() => $"{Position} of size {Size.X}x{Size.Y}";
        public bool Equals(Rectangle32 other) => Position.Equals(other.Position) && Size.Equals(other.Size);
    }
}
