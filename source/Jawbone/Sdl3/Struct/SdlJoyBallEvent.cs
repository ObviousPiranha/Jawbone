namespace Jawbone.Sdl3;

public struct SdlJoyBallEvent // SDL_JoyBallEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public byte Ball; // Uint8 ball
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
    public short Xrel; // Sint16 xrel
    public short Yrel; // Sint16 yrel
}
