using System;

namespace Jawbone;

public struct LineSegment32 : IEquatable<LineSegment32>
{
    public Point32 A;
    public Point32 B;

    public LineSegment32(Point32 a, Point32 b)
    {
        A = a;
        B = b;
    }

    public readonly bool Equals(LineSegment32 other) => A.Equals(other.A) && B.Equals(other.B);
    public override readonly bool Equals(object? obj) => obj is LineSegment32 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(A, B);
    public override readonly string ToString() => $"{A} to {B}";
}
