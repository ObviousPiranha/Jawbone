using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMouseMotionEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public uint MouseId;
    public uint Which;
    public uint State;
    public float X;
    public float Y;
    public float RelativeX;
    public float RelativeY;
}
