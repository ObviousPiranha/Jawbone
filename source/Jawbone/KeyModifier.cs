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
}
