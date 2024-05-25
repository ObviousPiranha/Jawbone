using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlDropEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public float X;
    public float Y;
    public CString Source;
    public CString Data;
}
