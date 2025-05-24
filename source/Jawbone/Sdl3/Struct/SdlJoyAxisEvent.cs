namespace Jawbone.Sdl3;

public struct SdlJoyAxisEvent // SDL_JoyAxisEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public byte Axis; // Uint8 axis
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
    public short Value; // Sint16 value
    public ushort Padding4; // Uint16 padding4
}
