using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlSensorEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public SensorData Data;
    public ulong TimestampUs;

    [InlineArray(6)]
    public struct SensorData
    {
        private float _a;
    }
}
