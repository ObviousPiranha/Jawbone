using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlSysWmEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public nint Msg;
}
