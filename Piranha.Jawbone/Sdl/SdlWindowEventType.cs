namespace Piranha.Jawbone.Sdl;

public enum SdlWindowEventType : byte
{
    Shown = 0x1,
    Hidden = 0x2,
    Exposed = 0x3,
    Moved = 0x4,
    Resized = 0x5,
    SizeChanged = 0x6,
    Minimized = 0x7,
    Maximized = 0x8,
    Restored = 0x9,
    Enter = 0xa,
    Leave = 0xb,
    FocusGained = 0xc,
    FocusLost = 0xd,
    Close = 0xe,
    TakeFocus = 0xf,
    HitTest = 0x10,
}
