using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlUserEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowsId;
    public int Code;
    public nint Data1;
    public nint Data2;
}
