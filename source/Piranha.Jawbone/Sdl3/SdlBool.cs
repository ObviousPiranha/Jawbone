using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Sdl3;

public struct SdlBool : IEquatable<SdlBool>
{
    public int Value;

    public readonly bool Equals(SdlBool other) => Convert.ToBoolean(Value) == Convert.ToBoolean(other.Value);
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is SdlBool other && Equals(other);
    public override readonly int GetHashCode() => Value == 0 ? 0 : 1;
    public override readonly string ToString() => Convert.ToBoolean(Value).ToString();

    public static implicit operator bool(SdlBool sdlBool) => Convert.ToBoolean(sdlBool.Value);
    public static implicit operator SdlBool(bool b) => new SdlBool { Value = Convert.ToInt32(b) };

    public static bool operator ==(SdlBool a, SdlBool b) => a.Equals(b);
    public static bool operator !=(SdlBool a, SdlBool b) => !a.Equals(b);
}
