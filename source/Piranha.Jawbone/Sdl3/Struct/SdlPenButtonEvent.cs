namespace Piranha.Jawbone.Sdl3;

public struct SdlPenButtonEvent // SDL_PenButtonEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public uint Which; // SDL_PenID which
    public uint PenState; // SDL_PenInputFlags pen_state
    public float X; // float x
    public float Y; // float y
    public byte Button; // Uint8 button
    public byte Down; // bool down
}
