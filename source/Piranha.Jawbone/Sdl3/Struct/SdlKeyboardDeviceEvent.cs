namespace Piranha.Jawbone.Sdl3;

public struct SdlKeyboardDeviceEvent // SDL_KeyboardDeviceEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_KeyboardID which
}
