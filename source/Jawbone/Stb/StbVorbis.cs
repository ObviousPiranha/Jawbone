using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Stb;

#pragma warning disable CA1401

public static partial class StbVorbis
{
    [LibraryImport(C.Library, EntryPoint = "stb_vorbis_decode_memory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int DecodeMemory(
        in byte mem,
        int length,
        out int channels,
        out int sampleRate,
        out nint output);

    [LibraryImport(C.Library, EntryPoint = "stb_vorbis_decode_filename", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int DecodeFilename(
        string filename,
        out int channels,
        out int sampleRate,
        out nint output);
}

#pragma warning restore CA1401
