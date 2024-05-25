using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMouseWheelEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public uint Which;
    public float X;
    public float Y;
    public SdlMouseWheelDirection Direction;
    public float MouseX;
    public float MouseY;
}
