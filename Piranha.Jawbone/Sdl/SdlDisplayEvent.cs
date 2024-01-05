using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlDisplayEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint Display;
    public SdlDisplayEventType Event;
    public byte Padding1;
    public byte Padding2;
    public byte Padding3;
    public int Data1;

    public readonly SdlOrientation Orientation => (SdlOrientation)Data1;
}
