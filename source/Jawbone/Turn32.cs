using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone;

public readonly struct Turn32 : IEquatable<Turn32>
{
    public float Value { get; private init; }
    public float Relative => 1f <= Value ? Value - 2f : Value;

    public Turn32(float value)
    {
        if (!float.IsFinite(value))
            Throw();
        value %= 2f;
        if (value < 0f)
            value += 2f;
        Value = value;
        static void Throw() => throw new ArgumentOutOfRangeException();
    }

    public float ToRadians() => Value * float.Pi;
    public float ToDegrees() => Value * 180f;

    public bool Equals(Turn32 other) => Value.Equals(other.Value);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Turn32 other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    public static Turn32 One => new() { Value = 1f };

    public static bool operator ==(Turn32 a, Turn32 b) => a.Equals(b);
    public static bool operator !=(Turn32 a, Turn32 b) => !a.Equals(b);
    public static Turn32 operator +(Turn32 a, Turn32 b) => new() { Value = a.Value + b.Value % 2f };
    public static Turn32 operator -(Turn32 a, Turn32 b) => new() { Value = a.Value + 2f - b.Value % 2f };
    public static Turn32 operator +(Turn32 a, float b) => new(a.Value + b);
    public static Turn32 operator -(Turn32 a, float b) => new(a.Value - b);
}