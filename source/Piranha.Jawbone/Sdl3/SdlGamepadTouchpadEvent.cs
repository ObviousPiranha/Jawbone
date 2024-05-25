using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlGamepadTouchpadEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint Which;
    public int Touchpad;
    public int Finger;
    public float X;
    public float Y;
    public float Pressure;
}
