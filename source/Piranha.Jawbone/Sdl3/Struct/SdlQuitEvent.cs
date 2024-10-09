namespace Piranha.Jawbone.Sdl3;

public struct SdlQuitEvent // SDL_QuitEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
}
