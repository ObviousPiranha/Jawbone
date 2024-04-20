using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlCommonEvent
{
    public SdlEventType Type;
    public uint Timestamp;
}
