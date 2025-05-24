namespace Jawbone.Sdl3;

public struct SdlPenProximityEvent // SDL_PenProximityEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public uint Which; // SDL_PenID which
}
