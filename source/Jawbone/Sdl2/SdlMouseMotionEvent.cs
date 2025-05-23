using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMouseMotionEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public uint Which;
    public uint State;
    public int X;
    public int Y;
    public int RelativeX;
    public int RelativeY;
}
