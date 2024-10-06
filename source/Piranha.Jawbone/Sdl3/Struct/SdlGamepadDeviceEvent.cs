namespace Piranha.Jawbone.Sdl3;

public struct SdlGamepadDeviceEvent // SDL_GamepadDeviceEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
}
