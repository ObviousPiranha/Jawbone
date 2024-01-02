using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlTextEditingExtEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public nint Text;
    public int Start;
    public int Length;
}
