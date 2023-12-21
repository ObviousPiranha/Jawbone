using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://en.wikipedia.org/wiki/IPv6_address
[StructLayout(LayoutKind.Sequential)]
public readonly struct AddressV6 : IAddress<AddressV6>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint LinkLocalMask() => BitConverter.IsLittleEndian ? 0x0000c0ff : 0xffc00000;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint LinkLocalSubnet() => BitConverter.IsLittleEndian ? 0x000080fe : 0xfe800000;

    public static AddressV6 Local { get; } = Create(static span => span[^1] = 1);

    private static readonly uint PrefixV4 = BitConverter.IsLittleEndian ? 0xffff0000 : 0x0000ffff;

    public static AddressV6 Create(params byte[] values) => new(values);

    public static AddressV6 Create(SpanAction<byte> action)
    {
        var result = default(AddressV6);
        var span = AsBytes(ref result);
        action.Invoke(span);
        return result;
    }

    public static AddressV6 Create<TState>(TState state, SpanAction<byte, TState> action)
    {
        var result = default(AddressV6);
        var span = AsBytes(ref result);
        action.Invoke(span, state);
        return result;
    }

    public static AddressV6 FromHostOrdering(ReadOnlySpan<ushort> groups)
    {
        var result = default(AddressV6);
        var outGroups = MemoryMarshal.Cast<AddressV6, ushort>(
            new Span<AddressV6>(ref result))[..^2];
        groups[..outGroups.Length].CopyTo(outGroups);

        if (BitConverter.IsLittleEndian)
        {
            foreach (ref var block in outGroups)
                block = BinaryPrimitives.ReverseEndianness(block);
        }

        return result;
    }

    private readonly uint _a;
    private readonly uint _b;
    private readonly uint _c;
    private readonly uint _d;
    private readonly uint _scopeId;

    public readonly uint ScopeId => _scopeId;
    public readonly bool IsDefault => _a == 0 && _b == 0 && _c == 0 && _d == 0;
    public readonly bool IsLinkLocal => (_a & LinkLocalMask()) == LinkLocalSubnet();
    public readonly bool IsLoopback => Equals(Local);
    public readonly bool IsIpV4Mapped => _a == 0 && _b == 0 && _c == PrefixV4;

    public AddressV6(ReadOnlySpan<byte> values) : this(values, 0)
    {
    }

    public AddressV6(ReadOnlySpan<byte> values, uint scopeId) : this()
    {
        var span = AsBytes(ref this);
        values.Slice(0, span.Length).CopyTo(span);
        _scopeId = scopeId;
    }

    private AddressV6(uint a, uint b, uint c, uint d, uint scopeId = 0)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
        _scopeId = scopeId;
    }

    public readonly bool TryMapV4(out AddressV4 address)
    {
        if (IsIpV4Mapped)
        {
            address = new(_d);
            return true;
        }
        else
        {
            address = default;
            return false;
        }
    }

    public readonly AddressV6 WithScopeId(uint scopeId) => new(_a, _b, _c, _d, scopeId);

    public readonly bool Equals(AddressV6 other)
    {
        return
            _a == other._a &&
            _b == other._b &&
            _c == other._c &&
            _d == other._d &&
            _scopeId == other._scopeId;
    }

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is AddressV6 other && Equals(other);
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
            builder.Append("::ffff:");
            new AddressV4(_d).AppendTo(builder);
            return;
        }

        int zeroIndex = 0;
        int zeroLength = 0;

        var span16 = MemoryMarshal.Cast<AddressV6, ushort>(
            new ReadOnlySpan<AddressV6>(in this))[..^2];
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

        var span = AsReadOnlyBytes(in this);

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

        if (_scopeId != 0)
            builder.Append('%').Append(_scopeId);
    }

    private static string? DoTheParse(ReadOnlySpan<char> originalInput, out AddressV6 result)
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
        // with how AddressV6 converts such addresses to strings.
        const string IntroV4 = "::ffff:";
        if (s.StartsWith(IntroV4) && AddressV4.TryParse(s[IntroV4.Length..], null, out var a32))
        {
            result = (AddressV6)a32;
            return null;
        }

        var scopeId = default(uint);
        {
            var scopeIndex = s.LastIndexOf('%');
            if (0 <= scopeIndex)
            {
                if (!uint.TryParse(s.Slice(scopeIndex + 1), out scopeId))
                {
                    result = default;
                    return "Invalid scope ID.";
                }

                s = s[..scopeIndex];
            }
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

        result = FromHostOrdering(blocks).WithScopeId(scopeId);
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

    public static AddressV6 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var exceptionMessage = DoTheParse(s, out var result);
        if (exceptionMessage is not null)
            throw new FormatException(exceptionMessage);
        return result;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out AddressV6 result)
    {
        var exceptionMessage = DoTheParse(s, out result);
        return exceptionMessage is null;
    }

    public static AddressV6 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        var exceptionMessage = DoTheParse(s, out var result);
        if (exceptionMessage is not null)
            throw new FormatException(exceptionMessage);
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out AddressV6 result)
    {
        var exceptionMessage = DoTheParse(s, out result);
        return exceptionMessage is null;
    }

    public static Span<byte> AsBytes(ref AddressV6 address)
    {
        return MemoryMarshal.AsBytes(
            new Span<AddressV6>(ref address)).Slice(0, 16);
    }

    public static ReadOnlySpan<byte> AsReadOnlyBytes(ref readonly AddressV6 address)
    {
        return MemoryMarshal.AsBytes(
            new ReadOnlySpan<AddressV6>(in address)).Slice(0, 16);
    }

    public static bool operator ==(AddressV6 a, AddressV6 b) => a.Equals(b);
    public static bool operator !=(AddressV6 a, AddressV6 b) => !a.Equals(b);
    public static explicit operator AddressV6(AddressV4 address) => new(0, 0, PrefixV4, (uint)address);
    public static explicit operator AddressV4(AddressV6 address)
    {
        if (!address.IsIpV4Mapped)
            throw new InvalidCastException("IPv6 address is not IPv4-mapped.");

        return new(address._d);
    }
}
