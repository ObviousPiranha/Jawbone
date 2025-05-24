using System.Runtime.InteropServices;

namespace Jawbone.Stb;

[StructLayout(LayoutKind.Sequential)]
public struct StbttPackContext
{
    public nint UserAllocatorContext;
    public nint PackInfo;
    public int Width;
    public int Height;
    public int StrideInBytes;
    public int Padding;
    public int SkipMissing;
    public uint HOversample;
    public uint VOversample;
    public nint Pixels;
    public nint Nodes;
}
