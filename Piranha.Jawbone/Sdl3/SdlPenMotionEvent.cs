using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlPenMotionEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public uint Which;
    public byte Padding1;
    public byte Padding2;
    public ushort PenState;
    public float X;
    public float Y;
    public PenAxisArray Axes;
}
