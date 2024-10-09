namespace Piranha.Jawbone.Sdl3;

public struct SdlMessageBoxData // SDL_MessageBoxData
{
    public uint Flags; // SDL_MessageBoxFlags flags
    public nint Window; // SDL_Window * window
    public nint Title; // char const * title
    public nint Message; // char const * message
    public int Numbuttons; // int numbuttons
    public nint Buttons; // SDL_MessageBoxButtonData const * buttons
    public nint ColorScheme; // SDL_MessageBoxColorScheme const * colorScheme
}
