namespace Piranha.Jawbone.Sdl3;

public struct SdlAudioDeviceEvent // SDL_AudioDeviceEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_AudioDeviceID which
    public byte Recording; // bool recording
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
