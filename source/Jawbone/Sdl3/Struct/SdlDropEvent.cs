namespace Jawbone.Sdl3;

public struct SdlDropEvent // SDL_DropEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public float X; // float x
    public float Y; // float y
    public CString Source; // char const * source
    public CString Data; // char const * data
}
