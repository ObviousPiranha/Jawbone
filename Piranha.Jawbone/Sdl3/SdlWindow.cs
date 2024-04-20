using System;

namespace Piranha.Jawbone.Sdl3;

[Flags]
public enum SdlWindow : uint
{
    FullScreen = 1u << 0,
    OpenGl = 1u << 1,
    Occluded = 1u << 2,
    Hidden = 1u << 3,
    Borderless = 1u << 4,
    Resizable = 1u << 5,
    Minimized = 1u << 6,
    Maximized = 1u << 7,
    MouseGrabbed = 1u << 8,
    InputFocus = 1u << 9,
    MouseFocus = 1u << 10,
    External = 1u << 11,
    HighPixelDensity = 1u << 13,
    MouseCapture = 1u << 14,
    AlwaysOnTop = 1u << 15,
    Utility = 1u << 17,
    Tooltip = 1u << 18,
    PopupMenu = 1u << 19,
    KeyboardGrabbed = 1u << 20,
    Vulkan = 1u << 28,
    Metal = 1u << 29,
    Transparent = 1u << 30,
    NotFocusable = 1u << 31
}
