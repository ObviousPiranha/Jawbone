using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlVersion
{
    public byte Major;
    public byte Minor;
    public byte Patch;

    public override string ToString() => $"{Major}.{Minor}.{Patch}";
}
