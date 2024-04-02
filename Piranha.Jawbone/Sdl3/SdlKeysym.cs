using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlKeysym
{
    public int Scancode;
    public int Sym;
    public ushort Mod;
    public uint Unused;
}
