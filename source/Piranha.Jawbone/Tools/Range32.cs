using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Range32 : IEquatable<Range32>
{
    public readonly int Start;
    public readonly int Length;

    public Range32(int start, int length)
    {
        Start = start;
        Length = length;
    }

    public readonly bool Equals(Range32 other) => Start == other.Start && Length == other.Length;
    public override readonly bool Equals(object? obj) => obj is Range32 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Start, Length);
    public override readonly string ToString() => $"start {Start} length {Length}";
}
