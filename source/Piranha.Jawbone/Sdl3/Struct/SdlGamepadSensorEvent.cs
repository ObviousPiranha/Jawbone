namespace Piranha.Jawbone.Sdl3;

public struct SdlGamepadSensorEvent // SDL_GamepadSensorEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_JoystickID which
    public int Sensor; // Sint32 sensor
    public nint Data; // float[3] data
    public ulong SensorTimestamp; // Uint64 sensor_timestamp
}
