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
    public static Address128 Local { get; } = Create(static span => span[^1] = 1);

    internal static readonly uint PrefixV4 = BitConverter.IsLittleEndian ? 0xffff0000 : 0x0000ffff;

    public static Address128 Create(params byte[] values) => new(values);

    public static Address128 Create(SpanAction<byte> action)
    {
        var result = default(Address128);
        var span = AsBytes(ref result);
        action.Invoke(span);
        return result;
    }

    public static Address128 Create<TState>(TState state, SpanAction<byte, TState> action)
    {
        var result = default(Address128);
        var span = AsBytes(ref result);
        action.Invoke(span, state);
        return result;
    }

    public static Address128 FromHostOrdering(ReadOnlySpan<ushort> groups)
    {
        var result = default(Address128);
        var outGroups = MemoryMarshal.Cast<Address128, ushort>(
            new Span<Address128>(ref result));
        groups[..outGroups.Length].CopyTo(outGroups);

        if (BitConverter.IsLittleEndian)
        {
            foreach (ref var block in outGroups)
                Address.SwapU16(ref block);
        }

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
        var span = AsBytes(ref this);
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
    public override readonly string ToString()
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

        var span16 = MemoryMarshal.Cast<Address128, ushort>(
            new ReadOnlySpan<Address128>(this));
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

        var span = AsReadOnlyBytes(this);
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

    public static Span<byte> AsBytes(ref Address128 address) => MemoryMarshal.AsBytes(new Span<Address128>(ref address));
    public static ReadOnlySpan<byte> AsReadOnlyBytes(in Address128 address) => MemoryMarshal.AsBytes(new ReadOnlySpan<Address128>(address));

    private static string? DoTheParse(ReadOnlySpan<char> originalInput, out Address128 result)
    {
        if (originalInput.IsEmpty)
        {
            result = default;
            return "Input string is empty.";
        }

        var s = originalInput;

        {
            var hasOpeningBracket = s[0] == '[';
            var hasClosingBracket = s[^1] == ']';

            if (hasOpeningBracket != hasClosingBracket)
            {
                result = default;
                return hasOpeningBracket ? "Missing closing bracket." : "Missing opening bracket.";
            }

            if (hasOpeningBracket)
                s = s[1..^1];
        }

        if (s.Length < 2)
        {
            result = default;
            return "Input string too short.";
        }

        // TODO: Make this properly flexible.
        // This would still miss plenty of other valid representations
        // for IPv4-mapped addresses, but for now, it is consistent
        // with how Address128 converts such addresses to strings.
        const string IntroV4 = "::ffff:";
        if (s.StartsWith(IntroV4) && Address32.TryParse(s[IntroV4.Length..], null, out var a32))
        {
            result = a32.MapToV6();
            return null;
        }

        Span<ushort> blocks = stackalloc ushort[8];
        var division = s.IndexOf("::");

        if (0 <= division)
        {
            if (!TryParseHexBlocks(s[..division], blocks, out var leftBlocksWritten))
            {
                result = default;
                return "Bad hex block.";
            }

            if (!TryParseHexBlocks(s[(division + 2)..], blocks[leftBlocksWritten..], out var rightBlocksWritten))
            {
                result = default;
                return "Bad hex block.";
            }

            if (leftBlocksWritten + rightBlocksWritten == blocks.Length)
            {
                result = default;
                return "Malformed representation.";
            }

            blocks.Slice(leftBlocksWritten, rightBlocksWritten).CopyTo(blocks[^rightBlocksWritten..]);
            blocks[leftBlocksWritten..^rightBlocksWritten].Clear();
        }
        else if (!TryParseHexBlocks(s, blocks, out var blocksWritten) || blocksWritten < blocks.Length)
        {
            result = default;
            return "Bad hex block.";
        }

        result = Address128.FromHostOrdering(blocks);
        return null;

        static bool TryParseHexBlocks(ReadOnlySpan<char> s, Span<ushort> blocks, out int blocksWritten)
        {
            blocksWritten = 0;

            if (s.IsEmpty)
                return true;

            if (!TryParseHexBlock(s, out var block))
                return false;

            blocks[0] = block;
            blocksWritten = 1;
            var index = HexLength(block);

            while (blocksWritten < blocks.Length)
            {
                if (index == s.Length)
                    return true;

                if (s[index] != ':' || !TryParseHexBlock(s[++index..], out block))
                    return false;

                blocks[blocksWritten++] = block;
                index += HexLength(block);
            }

            return index == s.Length;
        }

        static bool TryParseHexBlock(ReadOnlySpan<char> s, out ushort u16)
        {
            if (s.IsEmpty)
            {
                u16 = default;
                return false;
            }

            int result = HexDigit(s[0]);

            if (result == -1)
            {
                u16 = default;
                return false;
            }

            int digitCount = 1;

            while (digitCount < s.Length)
            {
                var nextDigit = HexDigit(s[digitCount]);

                if (nextDigit == -1)
                    break;

                if (4 < ++digitCount)
                {
                    u16 = default;
                    return false;
                }

                result = (result << 4) | nextDigit;
            }

            u16 = (ushort)result;
            return true;
        }

        static int HexDigit(int c)
        {
            if ('0' <= c && c <= '9')
                return c - '0';
            if ('a' <= c && c <= 'f')
                return c - 'a' + 10;
            if ('A' <= c && c <= 'F')
                return c - 'A' + 10;

            return -1;
        }

        static int HexLength(int n) => 0xfff < n ? 4 : 0xff < n ? 3 : 0xf < n ? 2 : 1;
    }

    public static Address128 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var exceptionMessage = DoTheParse(s, out var result);
        if (exceptionMessage is not null)
            throw new FormatException(exceptionMessage);
        return result;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Address128 result)
    {
        var exceptionMessage = DoTheParse(s, out result);
        return exceptionMessage is null;
    }

    public static Address128 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        var exceptionMessage = DoTheParse(s, out var result);
        if (exceptionMessage is not null)
            throw new FormatException(exceptionMessage);
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Address128 result)
    {
        var exceptionMessage = DoTheParse(s, out result);
        return exceptionMessage is null;
    }

    public static bool operator ==(Address128 a, Address128 b) => a.Equals(b);
    public static bool operator !=(Address128 a, Address128 b) => !a.Equals(b);
    public static Address128 operator ~(Address128 a) => new(~a._a, ~a._b, ~a._c, ~a._d);
    public static Address128 operator &(Address128 a, Address128 b) => new(a._a & b._a, a._b & b._b, a._c & b._c, a._d & b._d);
    public static Address128 operator |(Address128 a, Address128 b) => new(a._a | b._a, a._b | b._b, a._c | b._c, a._d | b._d);
    public static Address128 operator ^(Address128 a, Address128 b) => new(a._a ^ b._a, a._b ^ b._b, a._c ^ b._c, a._d ^ b._d);

    public static implicit operator Address128(AnyAddress anyAddress) => Any;
    public static implicit operator Address128(LocalAddress localAddress) => Local;
}
