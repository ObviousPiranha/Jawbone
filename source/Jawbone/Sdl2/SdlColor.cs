using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlColor
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;
}
