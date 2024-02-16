using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone;

public readonly struct BigEndianUInt32 : IEquatable<BigEndianUInt32>
{
    public readonly uint RawValue { get; init; }
    public readonly uint HostValue
    {
        get => BitConverter.IsLittleEndian ?
            BinaryPrimitives.ReverseEndianness(RawValue) :
            RawValue;
        
        init => RawValue = BitConverter.IsLittleEndian ?
            BinaryPrimitives.ReverseEndianness(value) :
            value;
    }

    public readonly bool Equals(BigEndianUInt32 other) => RawValue == other.RawValue;
    public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is BigEndianUInt32 other && Equals(other);
    public readonly override int GetHashCode() => RawValue.GetHashCode();
    public readonly override string? ToString() => HostValue.ToString();
}