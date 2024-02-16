using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone;

public readonly struct BigEndianInt32 : IEquatable<BigEndianInt32>
{
    public readonly int RawValue { get; init; }
    public readonly int HostValue
    {
        get => BitConverter.IsLittleEndian ?
            BinaryPrimitives.ReverseEndianness(RawValue) :
            RawValue;
        
        init => RawValue = BitConverter.IsLittleEndian ?
            BinaryPrimitives.ReverseEndianness(value) :
            value;
    }

    public readonly bool Equals(BigEndianInt32 other) => RawValue == other.RawValue;
    public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is BigEndianInt32 other && Equals(other);
    public readonly override int GetHashCode() => RawValue.GetHashCode();
    public readonly override string? ToString() => HostValue.ToString();
}