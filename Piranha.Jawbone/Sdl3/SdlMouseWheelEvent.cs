using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMouseWheelEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public uint Which;
    public int X;
    public int Y;
    public uint Direction;
    public float PreciseX;
    public float PreciseY;
    public int MouseX;
    public int MouseY;
}
