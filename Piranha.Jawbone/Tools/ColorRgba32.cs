using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct ColorRgba32 : IEquatable<ColorRgba32>, ISpanParsable<ColorRgba32>
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public ColorRgba32(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public readonly bool Equals(ColorRgba32 other) => R == other.R && G == other.G && B == other.B && A == other.A;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is ColorRgba32 other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(R, G, B, A);
    public override readonly string ToString()
    {
        return string.Create(
            9,
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
                span[7] = Utf16.GetHighHexDigit(state.A);
                span[8] = Utf16.GetLowHexDigit(state.A);
            });
    }

    public readonly void WriteUtf8(Span<byte> utf8)
    {
        utf8[8] = Utf8.GetLowHexDigit(A);
        utf8[7] = Utf8.GetHighHexDigit(A);
        utf8[6] = Utf8.GetLowHexDigit(B);
        utf8[5] = Utf8.GetHighHexDigit(B);
        utf8[4] = Utf8.GetLowHexDigit(G);
        utf8[3] = Utf8.GetHighHexDigit(G);
        utf8[2] = Utf8.GetLowHexDigit(R);
        utf8[1] = Utf8.GetHighHexDigit(R);
        utf8[0] = (byte)'#';
    }

    public static ColorRgba32 ParseUtf8(ReadOnlySpan<byte> utf8)
    {
        var offset = Convert.ToInt32(utf8[0] == '#');
        var r = Hex.ParseDigits(utf8[offset + 0], utf8[offset + 1]);
        var g = Hex.ParseDigits(utf8[offset + 2], utf8[offset + 3]);
        var b = Hex.ParseDigits(utf8[offset + 4], utf8[offset + 5]);
        var a = Hex.ParseDigits(utf8[offset + 6], utf8[offset + 7]);
        return new ColorRgba32((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public static ColorRgba32 Parse(
        string s,
        IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ColorRgba32 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static ColorRgba32 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var offset = Convert.ToInt32(s[0] == '#');
        var r = Hex.ParseDigits(s[offset + 0], s[offset + 1]);
        var g = Hex.ParseDigits(s[offset + 2], s[offset + 3]);
        var b = Hex.ParseDigits(s[offset + 4], s[offset + 5]);
        var a = Hex.ParseDigits(s[offset + 6], s[offset + 7]);
        return new ColorRgba32((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ColorRgba32 result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return false;
        }

        var offset = Convert.ToInt32(s[0] == '#');
        if (s.Length < 8 + offset)
        {
            result = default;
            return false;
        }

        var r = Hex.MaybeParseDigits(s[offset + 0], s[offset + 1]);
        var g = Hex.MaybeParseDigits(s[offset + 2], s[offset + 3]);
        var b = Hex.MaybeParseDigits(s[offset + 4], s[offset + 5]);
        var a = Hex.MaybeParseDigits(s[offset + 6], s[offset + 7]);

        if (r == Hex.InvalidDigit ||
            g == Hex.InvalidDigit ||
            b == Hex.InvalidDigit ||
            a == Hex.InvalidDigit)
        {
            result = default;
            return false;
        }

        result = new ColorRgba32(
            (byte)r,
            (byte)g,
            (byte)b,
            (byte)a);
        return true;
    }

    public static bool operator ==(ColorRgba32 a, ColorRgba32 b) => a.Equals(b);
    public static bool operator !=(ColorRgba32 a, ColorRgba32 b) => !a.Equals(b);
}
