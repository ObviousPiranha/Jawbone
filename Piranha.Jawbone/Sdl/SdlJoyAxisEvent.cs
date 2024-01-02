using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlJoyAxisEvent
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
