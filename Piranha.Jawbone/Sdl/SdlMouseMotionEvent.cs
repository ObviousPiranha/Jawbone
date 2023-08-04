using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMouseMotionEvent
{
    public uint Type;
    public uint Timestamp;
    public uint WindowId;
    public uint Which;
    public uint State;
    public int X;
    public int Y;
    public int RelativeX;
    public int RelativeY;
}
