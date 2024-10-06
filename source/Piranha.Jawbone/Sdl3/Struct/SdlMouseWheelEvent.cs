namespace Piranha.Jawbone.Sdl3;

public struct SdlMouseWheelEvent // SDL_MouseWheelEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public uint Which; // SDL_MouseID which
    public float X; // float x
    public float Y; // float y
    public SdlMouseWheelDirection Direction; // SDL_MouseWheelDirection direction
    public float MouseX; // float mouse_x
    public float MouseY; // float mouse_y
}
