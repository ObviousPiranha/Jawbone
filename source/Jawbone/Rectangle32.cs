using System;
using System.Runtime.InteropServices;

namespace Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Rectangle32 : IEquatable<Rectangle32>
{
    public Point32 Position;
    public Point32 Size;

    public Rectangle32(Point32 position, Point32 size)
    {
        Position = position;
        Size = size;
    }

    public override readonly bool Equals(object? obj) => obj is Rectangle32 r && Equals(r);
    public override readonly int GetHashCode() => HashCode.Combine(Position.X, Position.Y, Size.X, Size.Y);
    public override readonly string ToString() => $"position {Position} size {Size.X}x{Size.Y}";
    public readonly bool Equals(Rectangle32 other) => Position.Equals(other.Position) && Size.Equals(other.Size);

    public static bool operator ==(Rectangle32 a, Rectangle32 b) => a.Equals(b);
    public static bool operator !=(Rectangle32 a, Rectangle32 b) => !a.Equals(b);
}
