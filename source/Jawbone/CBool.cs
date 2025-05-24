using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone;

public struct CBool : IEquatable<CBool>
{
    public byte Value;

    public readonly bool Equals(CBool other) => Convert.ToBoolean(Value) == Convert.ToBoolean(other.Value);
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is CBool other && Equals(other);
    public override readonly int GetHashCode() => Value == 0 ? 0 : 1;
    public override readonly string ToString() => Convert.ToBoolean(Value).ToString();

    public static implicit operator bool(CBool CBool) => Convert.ToBoolean(CBool.Value);
    public static implicit operator CBool(bool b) => new() { Value = Convert.ToByte(b) };

    public static bool operator ==(CBool a, CBool b) => a.Equals(b);
    public static bool operator !=(CBool a, CBool b) => !a.Equals(b);
}
