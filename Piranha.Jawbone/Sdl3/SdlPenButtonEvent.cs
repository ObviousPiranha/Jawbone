using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlPenButtonEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public uint Which;
    public byte Button;
    public byte State;
    public ushort PenState;
    public float X;
    public float Y;
    public PenAxisArray Axes;
}
