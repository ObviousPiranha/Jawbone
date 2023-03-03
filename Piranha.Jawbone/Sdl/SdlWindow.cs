namespace Piranha.Jawbone.Sdl;

public static class SdlWindow
{
    public const uint FullScreen = 1 << 0;
    public const uint OpenGl = 1 << 1;
    public const uint Shown = 1 << 2;
    public const uint Hidden = 1 << 3;
    public const uint Borderless = 1 << 4;
    public const uint Resizable = 1 << 5;
    public const uint Minimized = 1 << 6;
    public const uint Maximized = 1 << 7;
    public const uint InputGrabbed = 1 << 8;
    public const uint InputFocus = 1 << 9;
    public const uint MouseFocus = 1 << 10;
    public const uint FullScreenDesktop = FullScreen | 1 << 12;
    public const uint Foreign = 1 << 11;
    public const uint AllowHighDpi = 1 << 13;
}
