using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlUserEvent
{
    public uint Type;
    public uint Timestamp;
    public uint WindowsId;
    public int Code;
    public nint Data1;
    public nint Data2;
}
