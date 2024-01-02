using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlQuitEvent
{
    public SdlEventType Type;
    public uint Timestamp;
}
