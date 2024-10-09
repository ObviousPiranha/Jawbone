namespace Piranha.Jawbone.Sdl3;

public struct SdlJoyButtonEvent // SDL_JoyButtonEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public byte Button; // Uint8 button
    public byte Down; // bool down
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
}
