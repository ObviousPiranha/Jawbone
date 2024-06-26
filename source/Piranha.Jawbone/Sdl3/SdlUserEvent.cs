using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlUserEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowsId;
    public int Code;
    public nint Data1;
    public nint Data2;
}
