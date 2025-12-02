using System;
using System.Runtime.InteropServices;

namespace Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Range32 : IEquatable<Range32>
{
    public int Start;
    public int Length;

    public Range32(int start, int length)
    {
        Start = start;
        Length = length;
    }

    public readonly bool Equals(Range32 other) => Start == other.Start && Length == other.Length;
    public override readonly bool Equals(object? obj) => obj is Range32 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Start, Length);
    public override readonly string ToString() => $"start {Start} length {Length}";

    public static bool operator ==(Range32 a, Range32 b) => a.Equals(b);
    public static bool operator !=(Range32 a, Range32 b) => !a.Equals(b);
}
