using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone.Sdl3;

public readonly struct KeyMapping : IEquatable<KeyMapping>
{
    public SdlScancode Scancode { get; init; }
    public KeyModifier Modifier { get; init; }

    public bool Equals(KeyMapping other) => Scancode == other.Scancode && Modifier == other.Modifier;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is KeyMapping other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Scancode, Modifier);
    public override string? ToString()
    {
        var mod = default(SdlKeymod);
        if (Modifier.MaskAll(KeyModifier.Control))
            mod |= SdlKeymod.Ctrl;
        if (Modifier.MaskAll(KeyModifier.Shift))
            mod |= SdlKeymod.Shift;
        if (Modifier.MaskAll(KeyModifier.Alt))
            mod |= SdlKeymod.Alt;
        if (Modifier.MaskAll(KeyModifier.Super))
            mod |= SdlKeymod.Gui;
        var key = Sdl.GetKeyFromScancode(Scancode, mod, false);
        var name = Sdl.GetKeyName(key);
        return name.ToString();
    }
}