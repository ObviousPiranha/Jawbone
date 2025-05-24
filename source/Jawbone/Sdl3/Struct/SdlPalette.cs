namespace Jawbone.Sdl3;

public struct SdlPalette // SDL_Palette
{
    public int Ncolors; // int ncolors
    public nint Colors; // SDL_Color * colors
    public uint Version; // Uint32 version
    public int Refcount; // int refcount
}
