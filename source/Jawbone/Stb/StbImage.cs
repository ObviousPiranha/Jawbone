using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Stb;

#pragma warning disable CA1401

public static partial class StbImage
{
    [LibraryImport(C.Library, EntryPoint = "stbi_load", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Load(
        string filename,
        out int x,
        out int y,
        out int comp,
        int reqComp);

    [LibraryImport(C.Library, EntryPoint = "stbi_load_from_memory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadFromMemory(
        in byte buffer,
        int len,
        out int x,
        out int y,
        out int comp,
        int reqComp);

    [LibraryImport(C.Library, EntryPoint = "stbi_image_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ImageFree(
        nint imageData);
}

#pragma warning restore CA1401