using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

// https://wiki.libsdl.org/SDL_DisplayMode
// SDL_video.h

[StructLayout(LayoutKind.Sequential)]
public struct SdlDisplayMode
{
    public uint Format;
    public int W;
    public int H;
    public int RefreshRate;
    public nint DriverData;
}
