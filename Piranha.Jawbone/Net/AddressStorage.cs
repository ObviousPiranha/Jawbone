using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net;

public readonly struct AddressStorage : IEquatable<AddressStorage>
{
    private readonly uint _a;
    private readonly uint _b;
    private readonly uint _c;
    private readonly uint _d;

    public AddressStorage(ReadOnlySpan<byte> values) : this()
    {
        var span = Address.GetSpanU8(this);
        values.Slice(0, Math.Min(values.Length, span.Length)).CopyTo(span);
    }

    internal AddressStorage(uint rawAddress) : this() => _a = rawAddress;
    internal AddressStorage(uint a, uint b, uint c, uint d)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
    }

    public bool Equals(AddressStorage other)
    {
        return
            _a == other._a &&
            _b == other._b &&
            _c == other._c &&
            _d == other._d;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is AddressStorage other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(_a, _b, _c, _d);
    public override string? ToString()
    {
        return base.ToString();
    }

    public static implicit operator AddressStorage(Address32 address)
        => new AddressStorage(address.RawAddress);
    public static implicit operator AddressStorage(Address128 address)
        => new AddressStorage(Address.GetReadOnlySpanU8(address));
}