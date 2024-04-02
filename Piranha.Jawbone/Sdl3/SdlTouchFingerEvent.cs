using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlTouchFingerEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public long TouchId;
    public long FingerId;
    public float X;
    public float Y;
    public float Dx;
    public float Dy;
    public float Pressure;
    public uint WindowId;
}
