using Jawbone.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlSurface
{
    public uint Flags;
    public nint Format;
    public int W;
    public int H;
    public int Pitch;
    public nint Pixels;
    public nint Userdata;
    public int Locked;
    public nint ListBlitmap;
    public SdlRect ClipRect;
    public nint Map;
    public int Refcount;

    public unsafe readonly Span<byte> GetPixelData()
    {
        return new Span<byte>(Pixels.ToPointer(), H * Pitch);
    }

    public static unsafe ref SdlSurface FromPointer(nint pointer)
    {
        if (pointer.IsInvalid())
            throw new ArgumentNullException(nameof(pointer), "Invalid pointer.");

        return ref Unsafe.AsRef<SdlSurface>(pointer.ToPointer());
    }
}
