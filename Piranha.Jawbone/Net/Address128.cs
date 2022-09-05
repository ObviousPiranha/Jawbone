using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://en.wikipedia.org/wiki/IPv6_address
[StructLayout(LayoutKind.Sequential)]
public readonly struct Address128 : IAddress<Address128>
{
    public static Address128 Create(params byte[] values) => new Address128(values);

    public static readonly Address128 Local = Create(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1);

    private readonly uint _a;
    private readonly uint _b;
    private readonly uint _c;
    private readonly uint _d;

    public byte this[int index] => Address.GetReadOnlySpanU8(this)[index];

    public Address128(ReadOnlySpan<byte> values) : this()
    {
        var span = Address.GetSpanU8(this);
        values.Slice(0, Math.Min(values.Length, span.Length)).CopyTo(span);
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