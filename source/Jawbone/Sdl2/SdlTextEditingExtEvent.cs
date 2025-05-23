using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

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
