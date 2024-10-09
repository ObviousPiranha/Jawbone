namespace Piranha.Jawbone.Sdl3;

public struct SdlSurface // SDL_Surface
{
    public uint Flags; // SDL_SurfaceFlags flags
    public SdlPixelFormat Format; // SDL_PixelFormat format
    public int W; // int w
    public int H; // int h
    public int Pitch; // int pitch
    public nint Pixels; // void * pixels
    public int Refcount; // int refcount
    public nint Reserved; // void * reserved
}
