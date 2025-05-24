using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlControllerSensorEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public int Sensor;
    public SensorData Data;
    public ulong TimestampUs;

    [InlineArray(3)]
    public struct SensorData
    {
        private float _a;
    }
}
