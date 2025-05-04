using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Net;

// https://en.wikipedia.org/wiki/IPv6_address
[StructLayout(LayoutKind.Explicit, Size = 20, Pack = 4)]
public struct AddressV6 : IAddress<AddressV6>
{
    [StructLayout(LayoutKind.Sequential)]
    [InlineArray(Length)]
    public struct ArrayU8
    {
        public const int Length = 16;
        private byte _first;
    }

    [StructLayout(LayoutKind.Sequential)]
    [InlineArray(Length)]
    public struct ArrayU16
    {
        public const int Length = 8;
        private ushort _first;
    }

    [StructLayout(LayoutKind.Sequential)]
    [InlineArray(Length)]
    public struct ArrayU32
    {
        public const int Length = 4;
        private uint _first;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint LinkLocalMask() => BitConverter.IsLittleEndian ? 0x0000c0ff : 0xffc00000;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint LinkLocalSubnet() => BitConverter.IsLittleEndian ? 0x000080fe : 0xfe800000;

    private static AddressV6 CreateLocal()
    {
        var result = default(AddressV6);
        result.DataU8[^1] = 1;
        return result;
    }

    public static AddressV6 Local { get; } = CreateLocal();

    private static readonly uint PrefixV4 = BitConverter.IsLittleEndian ? 0xffff0000 : 0x0000ffff;

    [FieldOffset(0)]
    public ArrayU8 DataU8;

    [FieldOffset(0)]
    public ArrayU16 DataU16;

    [FieldOffset(0)]
    public ArrayU32 DataU32;

    [FieldOffset(16)]
    public uint ScopeId;

    public readonly bool IsDefault => DataU32[0] == 0 && DataU32[1] == 0 && DataU32[2] == 0 && DataU32[3] == 0;
    public readonly bool IsLinkLocal => (DataU32[0] & LinkLocalMask()) == LinkLocalSubnet();
    public readonly bool IsLoopback => Equals(Local) || (TryMapV4(out var v4) && v4.IsLoopback);
    public readonly bool IsV4Mapped => DataU32[0] == 0 && DataU32[1] == 0 && DataU32[2] == PrefixV4;

    public AddressV6(ReadOnlySpan<byte> values) : this(values, 0)
    {
    }

    public AddressV6(ReadOnlySpan<byte> values, uint scopeId) : this()
    {
        values.Slice(0, ArrayU8.Length).CopyTo(DataU8);
        ScopeId = scopeId;
    }

    public AddressV6(ArrayU32 data, uint scopeId = 0)
    {
        DataU32 = data;
        ScopeId = scopeId;
    }

    private AddressV6(uint a, uint b, uint c, uint d, uint scopeId = 0)
    {
        DataU32[0] = a;
        DataU32[1] = b;
        DataU32[2] = c;
        DataU32[3] = d;
        ScopeId = scopeId;
    }

    public readonly bool TryMapV4(out AddressV4 address)
    {
        if (IsV4Mapped)
        {
            address = new(DataU32[3]);
            return true;
        }
        else
        {
            address = default;
            return false;
        }
    }

    public readonly bool Equals(AddressV6 other)
    {
        return
            DataU32[0] == other.DataU32[0] &&
            DataU32[1] == other.DataU32[1] &&
            DataU32[2] == other.DataU32[2] &&
            DataU32[3] == other.DataU32[3] &&
            ScopeId == other.ScopeId;
    }

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is AddressV6 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(DataU32[0], DataU32[1], DataU32[2], DataU32[3], ScopeId);
    public override readonly string ToString()
    {
        var builder = new StringBuilder(48);
        AppendTo(builder);
        return builder.ToString();
    }

    public readonly void AppendTo(StringBuilder builder)
    {
        if (IsV4Mapped)
        {
            builder.Append("::ffff:");
            new AddressV4(DataU32[3]).AppendTo(builder);
            return;
        }

        int zeroIndex = 0;
        int zeroLength = 0;

        for (int i = 0; i < ArrayU16.Length; ++i)
        {
            if (DataU16[i] == 0)
            {
                int j = i + 1;
                while (j < ArrayU16.Length && DataU16[j] == 0)
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

        if (1 < zeroLength)
        {
            var a = zeroIndex * 2;
            var b = (zeroIndex + zeroLength) * 2;
            builder
                .AppendV6Block(DataU8[..a])
                .Append("::")
                .AppendV6Block(DataU8[b..]);
        }
        else
        {
            builder.AppendV6Block(DataU8);
        }

        if (ScopeId != 0)
            builder.Append('%').Append(ScopeId);
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

        var blocks = default(ArrayU16);
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

            if (leftBlocksWritten + rightBlocksWritten == ArrayU16.Length)
            {
                result = default;
                return "Malformed representation.";
            }

            var end = leftBlocksWritten + rightBlocksWritten;
            blocks[leftBlocksWritten..end].CopyTo(blocks[^rightBlocksWritten..]);
            blocks[leftBlocksWritten..^rightBlocksWritten].Clear();
        }
        else if (!TryParseHexBlocks(s, blocks, out var blocksWritten) || blocksWritten < ArrayU16.Length)
        {
            result = default;
            return "Bad hex block.";
        }

        result = default;
        result.DataU16 = blocks;
        result.ScopeId = scopeId;

        if (BitConverter.IsLittleEndian)
        {
            foreach (ref var n in result.DataU16)
                n = BinaryPrimitives.ReverseEndianness(n);
        }
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

    public static Span<byte> AsBytes(ref AddressV6 address) => address.DataU8;
    public static ReadOnlySpan<byte> AsReadOnlyBytes(ref readonly AddressV6 address) => address.DataU8;

    public static bool operator ==(AddressV6 a, AddressV6 b) => a.Equals(b);
    public static bool operator !=(AddressV6 a, AddressV6 b) => !a.Equals(b);
    public static explicit operator AddressV6(AddressV4 address) => new(0, 0, PrefixV4, address.DataU32);
    public static explicit operator AddressV4(AddressV6 address)
    {
        if (!address.IsV4Mapped)
            throw new InvalidCastException("IPv6 address is not IPv4-mapped.");

        return new(address.DataU32[3]);
    }
}
