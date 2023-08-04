using Piranha.Jawbone.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Sdl;

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
            throw new ArgumentException("Invalid pointer.", nameof(pointer));

        return ref Unsafe.AsRef<SdlSurface>(pointer.ToPointer());
    }
}
