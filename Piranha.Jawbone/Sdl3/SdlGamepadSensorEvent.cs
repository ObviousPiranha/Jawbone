using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlGamepadSensorEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint Which;
    public int Sensor;
    public float Data0;
    public float Data1;
    public float Data2;
    public ulong SensorTimestamp;
}
