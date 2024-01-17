using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlColor
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;
}