using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlKeysym
{
    public int Scancode;
    public int Sym;
    public ushort Mod;
    public uint Unused;
}
