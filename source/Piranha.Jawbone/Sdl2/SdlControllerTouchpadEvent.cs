using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlControllerTouchpadEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public int Touchpad;
    public int Finger;
    public float X;
    public float Y;
    public float Pressure;
}
