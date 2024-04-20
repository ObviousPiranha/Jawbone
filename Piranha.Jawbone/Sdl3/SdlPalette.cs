using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlPalette
{
    public int NColors;
    public nint Colors;
    public uint Version;
    public int RefCount;

    public unsafe readonly ReadOnlySpan<SdlColor> GetColors() => new(Colors.ToPointer(), NColors);
}
