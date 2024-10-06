namespace Piranha.Jawbone.Sdl3;

public struct SdlSensorEvent // SDL_SensorEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint Which; // SDL_SensorID which
    public nint Data; // float[6] data
    public ulong SensorTimestamp; // Uint64 sensor_timestamp
}
