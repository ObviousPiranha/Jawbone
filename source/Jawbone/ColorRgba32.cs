using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct ColorRgba32 :
    IEquatable<ColorRgba32>,
    ISpanParsable<ColorRgba32>,
    IUtf8SpanParsable<ColorRgba32>,
    ISpanFormattable,
    IUtf8SpanFormattable
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public ColorRgba32(ColorRgb24 color) : this(color.R, color.G, color.B)
    {
    }

    public ColorRgba32(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
        A = byte.MaxValue;
    }

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

    public static ColorRgba32 Parse(
        string s,
        IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static ColorRgba32 Parse(string s) => Parse(s, null);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out ColorRgba32 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out ColorRgba32 result)
    {
        return TryParse(s, null, out result);
    }

    public static ColorRgba32 Parse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider = null)
    {
        var offset = Convert.ToInt32(s[0] == '#');
        var r = Hex.ParseDigits(s[offset + 0], s[offset + 1]);
        var g = Hex.ParseDigits(s[offset + 2], s[offset + 3]);
        var b = Hex.ParseDigits(s[offset + 4], s[offset + 5]);
        var a = Hex.ParseDigits(s[offset + 6], s[offset + 7]);
        return new ColorRgba32((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public static ColorRgba32 Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out ColorRgba32 result)
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

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out ColorRgba32 result)
    {
        return TryParse(s, null, out result);
    }

    public static ColorRgba32 Parse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider = null)
    {
        var offset = Convert.ToInt32(utf8Text[0] == '#');
        var r = Hex.ParseDigits(utf8Text[offset + 0], utf8Text[offset + 1]);
        var g = Hex.ParseDigits(utf8Text[offset + 2], utf8Text[offset + 3]);
        var b = Hex.ParseDigits(utf8Text[offset + 4], utf8Text[offset + 5]);
        var a = Hex.ParseDigits(utf8Text[offset + 6], utf8Text[offset + 7]);
        return new ColorRgba32((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out ColorRgba32 result)
    {
        if (utf8Text.IsEmpty)
        {
            result = default;
            return false;
        }

        var offset = Convert.ToInt32(utf8Text[0] == '#');
        if (utf8Text.Length < 8 + offset)
        {
            result = default;
            return false;
        }

        var r = Hex.MaybeParseDigits(utf8Text[offset + 0], utf8Text[offset + 1]);
        var g = Hex.MaybeParseDigits(utf8Text[offset + 2], utf8Text[offset + 3]);
        var b = Hex.MaybeParseDigits(utf8Text[offset + 4], utf8Text[offset + 5]);
        var a = Hex.MaybeParseDigits(utf8Text[offset + 6], utf8Text[offset + 7]);

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

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        out ColorRgba32 result)
    {
        return TryParse(utf8Text, null, out result);
    }

    public readonly bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        if (destination.Length < 9)
        {
            charsWritten = 0;
            return false;
        }

        destination[8] = Utf16.GetLowHexDigit(A);
        destination[7] = Utf16.GetHighHexDigit(A);
        destination[6] = Utf16.GetLowHexDigit(B);
        destination[5] = Utf16.GetHighHexDigit(B);
        destination[4] = Utf16.GetLowHexDigit(G);
        destination[3] = Utf16.GetHighHexDigit(G);
        destination[2] = Utf16.GetLowHexDigit(R);
        destination[1] = Utf16.GetHighHexDigit(R);
        destination[0] = '#';
        charsWritten = 9;
        return true;
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public readonly bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        if (utf8Destination.Length < 9)
        {
            bytesWritten = 0;
            return false;
        }

        utf8Destination[8] = Utf8.GetLowHexDigit(A);
        utf8Destination[7] = Utf8.GetHighHexDigit(A);
        utf8Destination[6] = Utf8.GetLowHexDigit(B);
        utf8Destination[5] = Utf8.GetHighHexDigit(B);
        utf8Destination[4] = Utf8.GetLowHexDigit(G);
        utf8Destination[3] = Utf8.GetHighHexDigit(G);
        utf8Destination[2] = Utf8.GetLowHexDigit(R);
        utf8Destination[1] = Utf8.GetHighHexDigit(R);
        utf8Destination[0] = (byte)'#';
        bytesWritten = 9;
        return true;
    }

    public static explicit operator ColorRgb24(ColorRgba32 c) => new(c.R, c.G, c.B);
    public static implicit operator ColorRgba32(ColorRgb24 c) => new(c);
    public static bool operator ==(ColorRgba32 a, ColorRgba32 b) => a.Equals(b);
    public static bool operator !=(ColorRgba32 a, ColorRgba32 b) => !a.Equals(b);
}
