using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://en.wikipedia.org/wiki/IPv6_address
[StructLayout(LayoutKind.Sequential)]
public readonly struct Address128 : IAddress<Address128>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint LinkLocalMask() => BitConverter.IsLittleEndian ? 0x0000c0ff : 0xffc00000;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint LinkLocalSubnet() => BitConverter.IsLittleEndian ? 0x000080fe : 0xfe800000;

    public static Address128 Any => default;
    public static Address128 Local { get; } = Create(static span => span[15] = 1);

    internal static readonly uint PrefixV4 = BitConverter.IsLittleEndian ? 0xffff0000 : 0x0000ffff;

    public static Address128 Create(params byte[] values) => new(values);

    public static Address128 Create(SpanAction<byte> action)
    {
        var result = default(Address128);
        var span = GetBytes(ref result);
        action.Invoke(span);
        return result;
    }

    public static Address128 Create<TState>(TState state, SpanAction<byte, TState> action)
    {
        var result = default(Address128);
        var span = GetBytes(ref result);
        action.Invoke(span, state);
        return result;
    }

    private readonly uint _a;
    private readonly uint _b;
    private readonly uint _c;
    private readonly uint _d;

    public readonly bool IsDefault => _a == 0 && _b == 0 && _c == 0 && _d == 0;
    public readonly bool IsLinkLocal => (_a & LinkLocalMask()) == LinkLocalSubnet();
    public readonly bool IsLoopback => Equals(Local);
    public readonly bool IsIpV4Mapped => _a == 0 && _b == 0 && _c == PrefixV4;

    public Address128(ReadOnlySpan<byte> values) : this()
    {
        var span = GetBytes(ref this);
        values.Slice(0, Math.Min(values.Length, span.Length)).CopyTo(span);
    }

    internal Address128(uint a, uint b, uint c, uint d)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
    }

    public readonly bool Equals(Address128 other)
    {
        return
            _a == other._a &&
            _b == other._b &&
            _c == other._c &&
            _d == other._d;
    }

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is Address128 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(_a, _b, _c, _d);
    public override readonly string? ToString()
    {
        var builder = new StringBuilder(48);
        AppendTo(builder);
        return builder.ToString();
    }

    public readonly void AppendTo(StringBuilder builder)
    {
        if (IsIpV4Mapped)
        {
            builder.Append("[::ffff:");
            new Address32(_d).AppendTo(builder);
            builder.Append(']');
            return;
        }

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

        var span = GetReadOnlyBytes(this);
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

    public static Span<byte> GetBytes(ref Address128 address) => Address.GetSpanU8(ref address);
    public static ReadOnlySpan<byte> GetReadOnlyBytes(in Address128 address) => Address.GetReadOnlySpanU8(address);

    public static Address128 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Address128 result)
    {
        throw new NotImplementedException();
    }

    public static Address128 Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Address128 result)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(Address128 a, Address128 b) => a.Equals(b);
    public static bool operator !=(Address128 a, Address128 b) => !a.Equals(b);
    public static Address128 operator &(Address128 a, Address128 b) => new(a._a & b._a, a._b & b._b, a._c & b._c, a._d & b._d);
    public static Address128 operator |(Address128 a, Address128 b) => new(a._a | b._a, a._b | b._b, a._c | b._c, a._d | b._d);
    public static Address128 operator ^(Address128 a, Address128 b) => new(a._a ^ b._a, a._b ^ b._b, a._c ^ b._c, a._d ^ b._d);
}
