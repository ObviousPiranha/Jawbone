using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlDollarGestureEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public long TouchId;
    public long GestureId;
    public uint NumFingers;
    public float Error;
    public float X;
    public float Y;
}
