namespace Piranha.Jawbone.Sdl3;

public struct SdlMouseDeviceEvent // SDL_MouseDeviceEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_MouseID which
}
