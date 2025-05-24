namespace Jawbone.Sdl3;

public struct SdlGamepadTouchpadEvent // SDL_GamepadTouchpadEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public int Touchpad; // Sint32 touchpad
    public int Finger; // Sint32 finger
    public float X; // float x
    public float Y; // float y
    public float Pressure; // float pressure
}
