using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlJoyBatteryEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public SdlJoystickPowerLevel Level;
}
