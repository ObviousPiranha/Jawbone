using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct SdlRect
{
    public int X;
    public int Y;
    public int W;
    public int H;

    public override readonly string ToString() => $"x {X} y {Y} w {W} h {H}";
}
