using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlJoyHatEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public byte Hat;
    public byte Value;
    public byte Padding1;
    public byte Padding2;
}
