using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlControllerDeviceEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public int Which;
}
