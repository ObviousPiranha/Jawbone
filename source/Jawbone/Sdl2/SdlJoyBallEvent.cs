using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlJoyBallEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public byte Ball;
    public byte Padding1;
    public byte Padding2;
    public byte Padding3;
    public short XRel;
    public short YRel;
}
