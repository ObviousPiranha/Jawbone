using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone.Sdl3;

public readonly struct KeyMapping :
    IEquatable<KeyMapping>,
    ISpanFormattable,
    IUtf8SpanFormattable,
    ISpanParsable<KeyMapping>,
    IUtf8SpanParsable<KeyMapping>
{
    public SdlScancode Scancode { get; init; }
    public KeyModifier Modifier { get; init; }

    public KeyMapping(
        SdlScancode scancode,
        KeyModifier modifier = default)
    {
        Scancode = scancode;
        Modifier = modifier;
    }

    public KeyMapping(SdlScancode scancode, SdlKeymod mod)
    {
        Scancode = scancode;
        var modifier = default(KeyModifier);
        CheckAndSet(SdlKeymod.Ctrl, KeyModifier.Control);
        CheckAndSet(SdlKeymod.Shift, KeyModifier.Shift);
        CheckAndSet(SdlKeymod.Alt, KeyModifier.Alt);
        CheckAndSet(SdlKeymod.Gui, KeyModifier.Super);
        Modifier = modifier;

        void CheckAndSet(SdlKeymod m, KeyModifier km)
        {
            if ((mod & m) != SdlKeymod.None)
                modifier |= km;
        }
    }

    public KeyMapping(in SdlKeyboardEvent sdlKeyboardEvent) : this(sdlKeyboardEvent.Scancode, sdlKeyboardEvent.Mod)
    {
    }

    public bool Equals(KeyMapping other) => Scancode == other.Scancode && Modifier == other.Modifier;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is KeyMapping other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Scancode, Modifier);
    public override string ToString()
    {
        var result = WithModifiers(Scancode.ToString());
        return result;
    }

    public string ToSdlString()
    {
        var key = Sdl.GetKeyFromScancode(Scancode, default, false);
        var name = Sdl.GetKeyName(key).ToString() ?? "";
        var result = WithModifiers(name);
        return result;
    }

    private string WithModifiers(string s)
    {
        var ctrl = Modifier.MaskAll(KeyModifier.Control) ? "Control " : "";
        var shift = Modifier.MaskAll(KeyModifier.Shift) ? "Shift " : "";
        var alt = Modifier.MaskAll(KeyModifier.Alt) ? "Alt " : "";
        var super = Modifier.MaskAll(KeyModifier.Super) ? "Super " : "";
        var result = string.Concat(ctrl, shift, alt, super, s);
        return result;
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
    {
        var writer = SpanWriter.Create(destination);
        var result =
            writer.TryFormat((int)Scancode) &&
            writer.TryWrite(':') &&
            writer.TryFormat((int)Modifier);
        if (result)
        {
            charsWritten = writer.Position;
            return true;
        }
        else
        {
            charsWritten = 0;
            return false;
        }
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[32];
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
    {
        var writer = SpanWriter.Create(utf8Destination);
        var result =
            writer.TryFormat((int)Scancode) &&
            writer.TryWrite((byte)':') &&
            writer.TryFormat((int)Modifier);
        if (result)
        {
            bytesWritten = writer.Position;
            return true;
        }
        else
        {
            bytesWritten = 0;
            return false;
        }
    }

    public static KeyMapping Parse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result))
            throw new FormatException();
        return result;
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out KeyMapping result)
    {
        var colon = s.IndexOf(':');
        if (0 <= colon &&
            int.TryParse(s[..colon], out var scancode) &&
            int.TryParse(s.Slice(colon + 1), out var modifier))
        {
            result = new((SdlScancode)scancode, (KeyModifier)modifier);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out KeyMapping result)
    {
        return TryParse(s, null, out result);
    }

    public static KeyMapping Parse(string s, IFormatProvider? provider = null)
    {
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out KeyMapping result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out KeyMapping result)
    {
        return TryParse(s, null, out result);
    }

    public static KeyMapping Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null)
    {
        if (!TryParse(utf8Text, provider, out var result))
            throw new FormatException();
        return result;
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out KeyMapping result)
    {
        var colon = utf8Text.IndexOf((byte)':');
        if (0 <= colon &&
            int.TryParse(utf8Text[..colon], out var scancode) &&
            int.TryParse(utf8Text.Slice(colon + 1), out var modifier))
        {
            result = new((SdlScancode)scancode, (KeyModifier)modifier);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        out KeyMapping result)
    {
        return TryParse(utf8Text, null, out result);
    }

    public static bool operator ==(KeyMapping a, KeyMapping b) => a.Equals(b);
    public static bool operator !=(KeyMapping a, KeyMapping b) => !a.Equals(b);
    public static implicit operator KeyMapping(SdlScancode scancode) => new(scancode);
}