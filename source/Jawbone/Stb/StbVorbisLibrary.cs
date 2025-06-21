// Code generated at 2025-06-21T19:21:55.

namespace Jawbone.Stb;

public sealed unsafe class StbVorbisLibrary
{
    private readonly nint _fp_DecodeFilename;
    private readonly nint _fp_DecodeMemory;

    public StbVorbisLibrary(
        System.Func<string, nint> loader)
    {
        _fp_DecodeFilename = loader.Invoke(nameof(DecodeFilename));
        _fp_DecodeMemory = loader.Invoke(nameof(DecodeMemory));
    }

    public int DecodeFilename(
        string filename,
        out int channels,
        out int sampleRate,
        out nint output)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, void*, void*, void*, int
            >)_fp_DecodeFilename;
        fixed (void* __p_channels = &channels)
        fixed (void* __p_sampleRate = &sampleRate)
        fixed (void* __p_output = &output)
        {
            var __result = __fp(filename, __p_channels, __p_sampleRate, __p_output);
            return __result;
        }
    }

    public int DecodeMemory(
        ref readonly byte mem,
        int length,
        out int channels,
        out int sampleRate,
        out nint output)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int, void*, void*, void*, int
            >)_fp_DecodeMemory;
        fixed (void* __p_mem = &mem)
        fixed (void* __p_channels = &channels)
        fixed (void* __p_sampleRate = &sampleRate)
        fixed (void* __p_output = &output)
        {
            var __result = __fp(__p_mem, length, __p_channels, __p_sampleRate, __p_output);
            return __result;
        }
    }
}
