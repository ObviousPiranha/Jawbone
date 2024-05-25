using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct ColorRgb24 : IEquatable<ColorRgb24>, ISpanParsable<ColorRgb24>
{
    public byte R;
    public byte G;
    public byte B;

    public ColorRgb24(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public readonly bool Equals(ColorRgb24 other) => R == other.R && G == other.G && B == other.B;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is ColorRgb24 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(R, G, B);
    public override readonly string ToString()
    {
        return string.Create(
            7,
            this,
            static (span, state) =>
            {
                span[0] = '#';
                span[1] = Utf16.GetHighHexDigit(state.R);
                span[2] = Utf16.GetLowHexDigit(state.R);
                span[3] = Utf16.GetHighHexDigit(state.G);
                span[4] = Utf16.GetLowHexDigit(state.G);
                span[5] = Utf16.GetHighHexDigit(state.B);
                span[6] = Utf16.GetLowHexDigit(state.B);
            });
    }

    public readonly void WriteUtf8(Span<byte> utf8)
    {
        utf8[6] = Utf8.GetLowHexDigit(B);
        utf8[5] = Utf8.GetHighHexDigit(B);
        utf8[4] = Utf8.GetLowHexDigit(G);
        utf8[3] = Utf8.GetHighHexDigit(G);
        utf8[2] = Utf8.GetLowHexDigit(R);
        utf8[1] = Utf8.GetHighHexDigit(R);
        utf8[0] = (byte)'#';
    }

    public static ColorRgb24 ParseUtf8(ReadOnlySpan<byte> utf8)
    {
        var offset = Convert.ToInt32(utf8[0] == '#');
        var r = Hex.ParseDigits(utf8[offset + 0], utf8[offset + 1]);
        var g = Hex.ParseDigits(utf8[offset + 2], utf8[offset + 3]);
        var b = Hex.ParseDigits(utf8[offset + 4], utf8[offset + 5]);
        return new ColorRgb24((byte)r, (byte)g, (byte)b);
    }

    public static ColorRgb24 Parse(
        string s,
        IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ColorRgb24 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static ColorRgb24 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var offset = Convert.ToInt32(s[0] == '#');
        var r = Hex.ParseDigits(s[offset + 0], s[offset + 1]);
        var g = Hex.ParseDigits(s[offset + 2], s[offset + 3]);
        var b = Hex.ParseDigits(s[offset + 4], s[offset + 5]);
        return new ColorRgb24((byte)r, (byte)g, (byte)b);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ColorRgb24 result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return false;
        }

        var offset = Convert.ToInt32(s[0] == '#');
        if (s.Length < 6 + offset)
        {
            result = default;
            return false;
        }

        var r = Hex.MaybeParseDigits(s[offset + 0], s[offset + 1]);
        var g = Hex.MaybeParseDigits(s[offset + 2], s[offset + 3]);
        var b = Hex.MaybeParseDigits(s[offset + 4], s[offset + 5]);

        if (r == Hex.InvalidDigit || g == Hex.InvalidDigit || b == Hex.InvalidDigit)
        {
            result = default;
            return false;
        }

        result = new ColorRgb24((byte)r, (byte)g, (byte)b);
        return true;
    }

    public static bool operator ==(ColorRgb24 a, ColorRgb24 b) => a.Equals(b);
    public static bool operator !=(ColorRgb24 a, ColorRgb24 b) => !a.Equals(b);
}
