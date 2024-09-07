namespace Piranha.Jawbone.Sdl2;

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
    public const uint MouseCapture = 1 << 14;
    public const uint AlwaysOnTop = 1 << 15;
    public const uint SkipTaskbar = 1 << 16;
    public const uint Utility = 1 << 17;
    public const uint Tooltip = 1 << 18;
    public const uint PopupMenu = 1 << 19;
    public const uint KeyboardGrabbed = 1 << 20;
    public const uint Vulkan = 1 << 28;
    public const uint Metal = 1 << 29;
}
