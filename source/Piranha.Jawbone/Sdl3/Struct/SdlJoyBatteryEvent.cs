namespace Piranha.Jawbone.Sdl3;

public struct SdlJoyBatteryEvent // SDL_JoyBatteryEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public SdlPowerState State; // SDL_PowerState state
    public int Percent; // int percent
}
