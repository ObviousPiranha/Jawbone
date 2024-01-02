namespace Piranha.Jawbone.Sdl;

public enum SdlWindowEventType : byte
{
    None,
    Shown,
    Hidden,
    Exposed,
    Moved,
    Resized,
    SizeChanged,
    Minimized,
    Maximized,
    Restored,
    Enter,
    Leave,
    FocusGained,
    FocusLost,
    Close,
    TakeFocus,
    HitTest,
    IccProfChanged,
    DisplayChanged
}
