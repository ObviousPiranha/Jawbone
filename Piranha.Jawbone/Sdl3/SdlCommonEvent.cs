using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlCommonEvent
{
    public SdlEventType Type;
    public uint Timestamp;
}
