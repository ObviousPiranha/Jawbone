using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlControllerAxisEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
    public byte Axis;
    public byte Padding1;
    public byte Padding2;
    public byte Padding3;
    public short Value;
    public ushort Padding4;
}
