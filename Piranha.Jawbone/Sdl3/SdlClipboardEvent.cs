using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlClipboardEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
}
