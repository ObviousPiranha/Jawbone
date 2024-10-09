namespace Piranha.Jawbone.Sdl3;

public struct SdlTextInputEvent // SDL_TextInputEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public CString Text; // char const * text
}
