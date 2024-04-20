using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlDisplayEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint DisplayId;
    public int Data1;

    public readonly SdlOrientation Orientation => (SdlOrientation)Data1;
}
