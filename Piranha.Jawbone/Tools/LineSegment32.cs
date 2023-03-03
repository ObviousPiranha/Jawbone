using System;

namespace Piranha.Jawbone.Tools;

public readonly struct LineSegment32 : IEquatable<LineSegment32>
{
    public readonly Point32 A;
    public readonly Point32 B;

    public LineSegment32(Point32 a, Point32 b)
    {
        A = a;
        B = b;
    }

    public bool Equals(LineSegment32 other) => A.Equals(other.A) && B.Equals(other.B);
    public override bool Equals(object? obj) => obj is LineSegment32 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(A, B);
    public override string? ToString() => $"{A} to {B}";
}
