using System;

namespace Piranha.Jawbone.Sdl3;

[Flags]
public enum SdlWindowFlags : ulong
{
    Fullscreen = 0x0000000000000001,
    OpenGl = 0x0000000000000002,
    Occluded = 0x0000000000000004,
    Hidden = 0x0000000000000008,
    Borderless = 0x0000000000000010,
    Resizable = 0x0000000000000020,
    Minimized = 0x0000000000000040,
    Maximized = 0x0000000000000080,
    MouseGrabbed = 0x0000000000000100,
    InputFocus = 0x0000000000000200,
    MouseFocus = 0x0000000000000400,
    External = 0x0000000000000800,
    Modal = 0x0000000000001000,
    HighPixelDensity = 0x0000000000002000,
    MouseCapture = 0x0000000000004000,
    MouseRelativeMode = 0x0000000000008000,
    AlwaysOnTop = 0x0000000000010000,
    Utility = 0x0000000000020000,
    Tooltip = 0x0000000000040000,
    PopupMenu = 0x0000000000080000,
    KeyboardGrabbed = 0x0000000000100000,
    Vulkan = 0x0000000010000000,
    Metal = 0x0000000020000000,
    Transparent = 0x0000000040000000,
    NotFocusable = 0x0000000080000000
}
