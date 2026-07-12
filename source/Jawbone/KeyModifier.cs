using System;

namespace Jawbone;

[Flags]
public enum KeyModifier
{
    None,
    Control = 1 << 0,
    Shift = 1 << 1,
    Alt = 1 << 2,
    Super = 1 << 3
}

public static class KeyModifierExtensions
{
    public static bool MaskAll(this KeyModifier keyModifier, KeyModifier mask) => (keyModifier & mask) == mask;
    public static bool MaskAny(this KeyModifier keyModifier, KeyModifier mask) => (keyModifier & mask) != KeyModifier.None;
    public static KeyModifier Create(Sdl3.SdlKeymod sdlKeymod)
    {
        var result = KeyModifier.None;
        if ((sdlKeymod & Sdl3.SdlKeymod.Ctrl) != Sdl3.SdlKeymod.None)
            result |= KeyModifier.Control;
        if ((sdlKeymod & Sdl3.SdlKeymod.Shift) != Sdl3.SdlKeymod.None)
            result |= KeyModifier.Shift;
        if ((sdlKeymod & Sdl3.SdlKeymod.Alt) != Sdl3.SdlKeymod.None)
            result |= KeyModifier.Alt;
        if ((sdlKeymod & Sdl3.SdlKeymod.Gui) != Sdl3.SdlKeymod.None)
            result |= KeyModifier.Super;
        return result;
    }
}
