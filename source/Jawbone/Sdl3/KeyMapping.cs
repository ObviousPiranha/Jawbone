using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Jawbone.Sdl3;

[JsonConverter(typeof(KeyMappingJsonConverter))]
public readonly struct KeyMapping : IEquatable<KeyMapping>
{
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

    public static bool operator ==(KeyMapping a, KeyMapping b) => a.Equals(b);
    public static bool operator !=(KeyMapping a, KeyMapping b) => !a.Equals(b);
}