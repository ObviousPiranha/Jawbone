using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlJoyButtonEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public byte Button;
    public byte State;
    public byte Padding1;
    public byte Padding2;
}
