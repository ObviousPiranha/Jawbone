namespace Jawbone.Sdl3;

public struct SdlClipboardEvent // SDL_ClipboardEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
}
