using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone;

public readonly struct Fixed24_8 : IEquatable<Fixed24_8>, IComparable<Fixed24_8>
{
    private readonly int _value;

    public int BigPart => (0 <= _value ? _value : _value + 255) >> 8;
    public int SmallPart
    {
        get

        {
            var masked = _value & 0xff;
            return 0 <= _value || masked == 0 ? masked : masked - 256;
        }
    }
    public int LeftWall => _value >> 8;
    public int Offset => _value & 0xff;

    private Fixed24_8(int value) => _value = value;

    public int CompareTo(Fixed24_8 other) => _value.CompareTo(other._value);
    public bool Equals(Fixed24_8 other) => _value == other._value;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Fixed24_8 other && Equals(other);
    public override int GetHashCode() => _value.GetHashCode();
    public override string ToString() => (_value / 256d).ToString();

    public static Fixed24_8 Zero => default;
    public static Fixed24_8 Unit => new(1);
    public static Fixed24_8 MinValue => new(int.MinValue);
    public static Fixed24_8 MaxValue => new(int.MaxValue);

    public static Fixed24_8 FromUnits(int units) => new(units);

    public static explicit operator int(Fixed24_8 value) => value.BigPart;
    public static explicit operator Fixed24_8(int value) => new(value << 8);
    public static explicit operator float(Fixed24_8 value) => value._value / 256f;
    public static explicit operator Fixed24_8(float value) => new((int)(value * 256f));
    public static explicit operator double(Fixed24_8 value) => value._value / 256d;
    public static explicit operator Fixed24_8(double value) => new((int)(value * 256d));
    public static bool operator ==(Fixed24_8 a, Fixed24_8 b) => a.Equals(b);
    public static bool operator !=(Fixed24_8 a, Fixed24_8 b) => !a.Equals(b);
    public static bool operator <(Fixed24_8 a, Fixed24_8 b) => a._value < b._value;
    public static bool operator >(Fixed24_8 a, Fixed24_8 b) => a._value > b._value;
    public static bool operator <=(Fixed24_8 a, Fixed24_8 b) => a._value <= b._value;
    public static bool operator >=(Fixed24_8 a, Fixed24_8 b) => a._value >= b._value;
    public static Fixed24_8 operator -(Fixed24_8 value) => new(-value._value);
    public static Fixed24_8 operator +(Fixed24_8 a, Fixed24_8 b) => new(a._value + b._value);
    public static Fixed24_8 operator -(Fixed24_8 a, Fixed24_8 b) => new(a._value - b._value);
    public static Fixed24_8 operator *(Fixed24_8 a, int b) => new(a._value * b);
    public static Fixed24_8 operator /(Fixed24_8 a, int b) => new(a._value / b);
}
