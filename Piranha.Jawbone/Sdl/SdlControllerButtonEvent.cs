using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlControllerButtonEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public byte Button;
    public byte State;
    public byte Padding1;
    public byte Padding2;
}
