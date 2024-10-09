namespace Piranha.Jawbone.Sdl3;

public struct SdlMouseButtonEvent // SDL_MouseButtonEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public uint Which; // SDL_MouseID which
    public byte Button; // Uint8 button
    public byte Down; // bool down
    public byte Clicks; // Uint8 clicks
    public byte Padding; // Uint8 padding
    public float X; // float x
    public float Y; // float y
}
