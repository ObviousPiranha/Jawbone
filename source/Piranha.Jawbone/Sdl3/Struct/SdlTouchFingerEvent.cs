namespace Piranha.Jawbone.Sdl3;

public struct SdlTouchFingerEvent // SDL_TouchFingerEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public ulong TouchID; // SDL_TouchID touchID
    public ulong FingerID; // SDL_FingerID fingerID
    public float X; // float x
    public float Y; // float y
    public float Dx; // float dx
    public float Dy; // float dy
    public float Pressure; // float pressure
    public uint WindowID; // SDL_WindowID windowID
}
