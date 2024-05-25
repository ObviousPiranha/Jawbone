using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlTextInputEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public CString Text;
}
