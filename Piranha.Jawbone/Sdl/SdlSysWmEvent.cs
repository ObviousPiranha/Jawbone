using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlSysWmEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public nint Msg;
}
