namespace Jawbone.Sdl3;

public struct SdlKeyboardEvent // SDL_KeyboardEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public uint Which; // SDL_KeyboardID which
    public SdlScancode Scancode; // SDL_Scancode scancode
    public uint Key; // SDL_Keycode key
    public SdlKeymod Mod; // SDL_Keymod mod
    public ushort Raw; // Uint16 raw
    public byte Down; // bool down
    public byte Repeat; // bool repeat
}
