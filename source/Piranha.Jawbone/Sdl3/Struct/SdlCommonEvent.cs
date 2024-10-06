namespace Piranha.Jawbone.Sdl3;

public struct SdlCommonEvent // SDL_CommonEvent
{
    public SdlEventType Type; // Uint32 type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
}
