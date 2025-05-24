using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlDropEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public nint File;
    public uint WindowId;
}
