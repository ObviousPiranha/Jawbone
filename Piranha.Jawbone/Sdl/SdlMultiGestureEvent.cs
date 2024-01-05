using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMultiGestureEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public long TouchId;
    public float DTheta;
    public float DDist;
    public float X;
    public float Y;
    public ushort NumFingers;
    public ushort Padding;
}
