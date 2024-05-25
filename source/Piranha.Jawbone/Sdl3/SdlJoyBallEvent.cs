using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlJoyBallEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint Which;
    public byte Ball;
    public byte Padding1;
    public byte Padding2;
    public byte Padding3;
    public short XRel;
    public short YRel;
}
