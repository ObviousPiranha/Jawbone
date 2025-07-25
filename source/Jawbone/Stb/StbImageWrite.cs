using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Stb;

#pragma warning disable CA1401

public static partial class StbImageWrite
{
    [LibraryImport(C.Library, EntryPoint = "stbi_write_png", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int WritePng(
        string filename,
        int x,
        int y,
        int comp,
        nint data,
        int strideBytes);

    [LibraryImport(C.Library, EntryPoint = "stbi_write_png", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int WritePng(
        string filename,
        int x,
        int y,
        int comp,
        in byte data,
        int strideBytes);
}

#pragma warning restore CA1401
