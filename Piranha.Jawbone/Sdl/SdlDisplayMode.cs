using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

// https://wiki.libsdl.org/SDL_DisplayMode
// SDL_video.h

[StructLayout(LayoutKind.Sequential)]
public struct SdlDisplayMode
{
    public uint format;
    public int w;
    public int h;
    public int refreshRate;
    public IntPtr driverData;
}
