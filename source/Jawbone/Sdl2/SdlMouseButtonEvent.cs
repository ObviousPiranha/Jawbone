using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlMouseButtonEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public uint Which;
    public byte Button;
    public byte State;
    public byte Clicks;
    public byte Padding1;
    public int X;
    public int Y;
}
