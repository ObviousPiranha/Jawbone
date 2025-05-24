namespace Jawbone.Sdl3;

public struct SdlTextEditingEvent // SDL_TextEditingEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public nint Text; // char const * text
    public int Start; // Sint32 start
    public int Length; // Sint32 length
}
