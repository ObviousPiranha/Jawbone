using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://en.wikipedia.org/wiki/IPv6_address
[StructLayout(LayoutKind.Sequential)]
public readonly struct Address128 : IAddress<Address128>
{
    public static readonly Address128 Any = default(Address128);
    public static readonly Address128 Local = Create(span => span[15] = 1);
    private static readonly uint PrefixV4 = BitConverter.IsLittleEndian ? 0xffff0000 : 0x0000ffff;

    public static Address128 Create(params byte[] values) => new Address128(values);

    public static Address128 Create(SpanAction<byte> action)
    {
        var result = default(Address128);
        var span = Address.GetSpanU8(result);
        action.Invoke(span);
        return result;
    }

    public static Address128 Create<TState>(TState state, SpanAction<byte, TState> action)
    {
        var result = default(Address128);
        var span = Address.GetSpanU8(result);
        action.Invoke(span, state);
        return result;
    }

    public static Address128 Map(Address32 address) => new(0, 0, PrefixV4, address.RawAddress);

    private readonly uint _a;
    private readonly uint _b;
    private readonly uint _c;
    private readonly uint _d;

    public readonly bool IsDefault => _a == 0 && _b == 0 && _c == 0 && _d == 0;
    public readonly bool IsIpv4Mapped => _a == 0 && _b == 0 && _c == PrefixV4;

    public byte this[int index] => Address.GetReadOnlySpanU8(this)[index];

    public Address128(ReadOnlySpan<byte> values) : this()
    {
        var span = Address.GetSpanU8(this);
        values.Slice(0, Math.Min(values.Length, span.Length)).CopyTo(span);
    }

    private Address128(uint a, uint b, uint c, uint d)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
    }

    public bool Equals(Address128 other)
    {
        return
            _a == other._a &&
            _b == other._b &&
            _c == other._c &&
            _d == other._d;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Address128 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(_a, _b, _c, _d);
    public override string? ToString()
    {
        var builder = new StringBuilder(48);
        AppendTo(builder);
        return builder.ToString();
    }

    public void AppendTo(StringBuilder builder)
    {
        int zeroIndex = 0;
        int zeroLength = 0;

        var span16 = Address.GetReadOnlySpanU16(this);
        for (int i = 0; i < span16.Length; ++i)
        {
            if (span16[i] == 0)
            {
                int j = i + 1;
                while (j < span16.Length && span16[j] == 0)
                    ++j;

                var length = j - i;

                if (zeroLength < length)
                {
                    zeroIndex = i;
                    zeroLength = length;
                }

                i = j;
            }
        }

        var span = Address.GetReadOnlySpanU8(this);
        builder.Append('[');

        if (1 < zeroLength)
        {
            builder
                .AppendV6Block(span.Slice(0, zeroIndex * 2))
                .Append("::")
                .AppendV6Block(span.Slice((zeroIndex + zeroLength) * 2));
        }
        else
        {
            builder.AppendV6Block(span);
        }

        builder.Append(']');
    }
}
