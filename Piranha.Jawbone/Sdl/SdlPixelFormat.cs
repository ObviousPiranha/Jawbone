using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlPixelFormat
{
    public uint Format;
    public nint Palette;
    public byte BitsPerPixel;
    public byte BytesPerPixel;
    public uint RMask;
    public uint GMask;
    public uint BMask;
    public uint AMask;
    public byte RLoss;
    public byte GLoss;
    public byte BLoss;
    public byte ALoss;
    public byte RShift;
    public byte GShift;
    public byte BShift;
    public byte AShift;
    public int RefCount;
    public nint Next;
}