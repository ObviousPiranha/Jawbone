// Code generated at 2025-06-21T19:19:00.

namespace Jawbone.Stb;

public sealed unsafe class StbImageWriteLibrary
{
    private readonly nint _fp_WritePng;

    public StbImageWriteLibrary(
        System.Func<string, nint> loader)
    {
        _fp_WritePng = loader.Invoke(nameof(WritePng));
    }

    public int WritePng(
        string filename,
        int x,
        int y,
        int comp,
        nint data,
        int strideBytes)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, int, int, int, nint, int, int
            >)_fp_WritePng;
        var __result = __fp(filename, x, y, comp, data, strideBytes);
        return __result;
    }

    public int WritePng(
        string filename,
        int x,
        int y,
        int comp,
        ref readonly byte data,
        int strideBytes)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, int, int, int, void*, int, int
            >)_fp_WritePng;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(filename, x, y, comp, __p_data, strideBytes);
            return __result;
        }
    }
}
