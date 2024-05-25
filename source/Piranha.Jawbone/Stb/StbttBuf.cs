using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Stb;

[StructLayout(LayoutKind.Sequential)]
public struct StbttBuf
{
    public nint Data;
    public int Cursor;
    public int Size;
}
