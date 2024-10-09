namespace Piranha.Jawbone.Sdl3;

public struct SdlMouseMotionEvent // SDL_MouseMotionEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public uint Which; // SDL_MouseID which
    public uint State; // SDL_MouseButtonFlags state
    public float X; // float x
    public float Y; // float y
    public float Xrel; // float xrel
    public float Yrel; // float yrel
}
