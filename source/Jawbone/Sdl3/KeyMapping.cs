using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Jawbone.Sdl3;

[JsonConverter(typeof(KeyMappingJsonConverter))]
public readonly struct KeyMapping : IEquatable<KeyMapping>
{
    private static ReadOnlySpan<byte> Ctrl => "Ctrl"u8;
    private static ReadOnlySpan<byte> Shift => "Shift"u8;
    private static ReadOnlySpan<byte> Alt => "Alt"u8;
    private static ReadOnlySpan<byte> Super => "Super"u8;

    public SdlScancode Scancode { get; init; }
    public KeyModifier Modifier { get; init; }

    public KeyMapping(SdlScancode scancode, KeyModifier modifier = default)
    {
        Scancode = scancode;
        Modifier = modifier;
    }

    public KeyMapping(SdlScancode scancode, SdlKeymod mod)
    {
        Scancode = scancode;
        if (IsSet(SdlKeymod.Ctrl))
            Modifier |= KeyModifier.Control;
        if (IsSet(SdlKeymod.Shift))
            Modifier |= KeyModifier.Shift;
        if (IsSet(SdlKeymod.Alt))
            Modifier |= KeyModifier.Alt;
        if (IsSet(SdlKeymod.Gui))
            Modifier |= KeyModifier.Super;
        bool IsSet(SdlKeymod m) => (mod & m) != SdlKeymod.None;
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

    public bool TryWriteUtf8Json(ref SpanWriter<byte> writer)
    {
        if (Modifier.MaskAll(KeyModifier.Control))
            writer.Write(Ctrl);
        if (Modifier.MaskAll(KeyModifier.Shift))
            writer.Write(Shift);
        if (Modifier.MaskAll(KeyModifier.Alt))
            writer.Write(Alt);
        if (Modifier.MaskAll(KeyModifier.Super))
            writer.Write(Super);
        var result = writer.TryFormat((int)Scancode);
        return result;
    }

    public static KeyMapping ParseUtf8Json(ReadOnlySpan<byte> utf8)
    {
        var reader = SpanReader.Create(utf8);
        var mod = default(KeyModifier);
        if (reader.TryMatch(Ctrl))
            mod |= KeyModifier.Control;
        if (reader.TryMatch(Shift))
            mod |= KeyModifier.Shift;
        if (reader.TryMatch(Alt))
            mod |= KeyModifier.Alt;
        if (reader.TryMatch(Super))
            mod |= KeyModifier.Super;
        var id = int.Parse(reader.Pending);
        var scancode = (SdlScancode)id;
        var result = new KeyMapping(scancode, mod);
        return result;
    }

    public static bool operator ==(KeyMapping a, KeyMapping b) => a.Equals(b);
    public static bool operator !=(KeyMapping a, KeyMapping b) => !a.Equals(b);
}