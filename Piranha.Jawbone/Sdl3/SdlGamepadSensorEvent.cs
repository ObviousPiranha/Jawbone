using System.Runtime.CompilerServices;
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
    public SensorData Data;
    public ulong SensorTimestamp;

    [InlineArray(Length)]
    public struct SensorData
    {
        public const int Length = 3;

        private float _a;
    }
}
