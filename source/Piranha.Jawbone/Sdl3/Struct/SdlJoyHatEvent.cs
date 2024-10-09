namespace Piranha.Jawbone.Sdl3;

public struct SdlJoyHatEvent // SDL_JoyHatEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public byte Hat; // Uint8 hat
    public byte Value; // Uint8 value
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
}
