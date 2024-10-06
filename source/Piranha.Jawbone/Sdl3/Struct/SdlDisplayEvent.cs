namespace Piranha.Jawbone.Sdl3;

public struct SdlDisplayEvent // SDL_DisplayEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint DisplayID; // SDL_DisplayID displayID
    public int Data1; // Sint32 data1
    public int Data2; // Sint32 data2
}
