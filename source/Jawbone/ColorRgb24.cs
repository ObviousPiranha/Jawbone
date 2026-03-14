using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct ColorRgb24 :
    IEquatable<ColorRgb24>,
    ISpanParsable<ColorRgb24>,
    IUtf8SpanParsable<ColorRgb24>,
    ISpanFormattable,
    IUtf8SpanFormattable
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

    public static ColorRgb24 Parse(
        string s,
        IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out ColorRgb24 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out ColorRgb24 result)
    {
        return TryParse(s.AsSpan(), null, out result);
    }

    public static ColorRgb24 Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
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
        out ColorRgb24 result)
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

    public static bool TryParse(ReadOnlySpan<char> s, out ColorRgb24 result) => TryParse(s, null, out result);

    public static ColorRgb24 Parse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider = null)
    {
        var offset = Convert.ToInt32(utf8Text[0] == '#');
        var r = Hex.ParseDigits(utf8Text[offset + 0], utf8Text[offset + 1]);
        var g = Hex.ParseDigits(utf8Text[offset + 2], utf8Text[offset + 3]);
        var b = Hex.ParseDigits(utf8Text[offset + 4], utf8Text[offset + 5]);
        return new ColorRgb24((byte)r, (byte)g, (byte)b);
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out ColorRgb24 result)
    {
        if (utf8Text.IsEmpty)
        {
            result = default;
            return false;
        }

        var offset = Convert.ToInt32(utf8Text[0] == '#');
        if (utf8Text.Length < 6 + offset)
        {
            result = default;
            return false;
        }

        var r = Hex.MaybeParseDigits(utf8Text[offset + 0], utf8Text[offset + 1]);
        var g = Hex.MaybeParseDigits(utf8Text[offset + 2], utf8Text[offset + 3]);
        var b = Hex.MaybeParseDigits(utf8Text[offset + 4], utf8Text[offset + 5]);

        if (r == Hex.InvalidDigit || g == Hex.InvalidDigit || b == Hex.InvalidDigit)
        {
            result = default;
            return false;
        }

        result = new ColorRgb24((byte)r, (byte)g, (byte)b);
        return true;
    }

    public static bool TryParse(ReadOnlySpan<byte> utf8Text, out ColorRgb24 result) => TryParse(utf8Text, null, out result);

    public readonly bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        if (destination.Length < 7)
        {
            charsWritten = 0;
            return false;
        }

        destination[6] = Utf16.GetLowHexDigit(B);
        destination[5] = Utf16.GetHighHexDigit(B);
        destination[4] = Utf16.GetLowHexDigit(G);
        destination[3] = Utf16.GetHighHexDigit(G);
        destination[2] = Utf16.GetLowHexDigit(R);
        destination[1] = Utf16.GetHighHexDigit(R);
        destination[0] = '#';
        charsWritten = 7;
        return true;
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public readonly bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
    {
        if (utf8Destination.Length < 7)
        {
            bytesWritten = 0;
            return false;
        }

        utf8Destination[6] = Utf8.GetLowHexDigit(B);
        utf8Destination[5] = Utf8.GetHighHexDigit(B);
        utf8Destination[4] = Utf8.GetLowHexDigit(G);
        utf8Destination[3] = Utf8.GetHighHexDigit(G);
        utf8Destination[2] = Utf8.GetLowHexDigit(R);
        utf8Destination[1] = Utf8.GetHighHexDigit(R);
        utf8Destination[0] = (byte)'#';
        bytesWritten = 7;
        return true;
    }

    public static bool operator ==(ColorRgb24 a, ColorRgb24 b) => a.Equals(b);
    public static bool operator !=(ColorRgb24 a, ColorRgb24 b) => !a.Equals(b);
}
