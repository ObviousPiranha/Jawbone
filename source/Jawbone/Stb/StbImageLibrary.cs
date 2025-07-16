// Code generated at 2025-06-21T19:16:28.

namespace Jawbone.Stb;

public sealed unsafe class StbImageLibrary
{
    private readonly nint _fp_ImageFree;
    private readonly nint _fp_Load;
    private readonly nint _fp_LoadFromMemory;

    public StbImageLibrary(
        System.Func<string, nint> loader)
    {
        _fp_ImageFree = loader.Invoke(nameof(ImageFree));
        _fp_Load = loader.Invoke(nameof(Load));
        _fp_LoadFromMemory = loader.Invoke(nameof(LoadFromMemory));
    }

    public void ImageFree(
        nint imageData)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_ImageFree;
        __fp(imageData);
    }

    public nint Load(
        string filename,
        out int x,
        out int y,
        out int comp,
        int reqComp)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, void*, void*, void*, int, nint
            >)_fp_Load;
        fixed (void* __p_x = &x)
        fixed (void* __p_y = &y)
        fixed (void* __p_comp = &comp)
        {
            var __result = __fp(filename, __p_x, __p_y, __p_comp, reqComp);
            return __result;
        }
    }

    public nint LoadFromMemory(
        ref readonly byte buffer,
        int len,
        out int x,
        out int y,
        out int comp,
        int reqComp)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int, void*, void*, void*, int, nint
            >)_fp_LoadFromMemory;
        fixed (void* __p_buffer = &buffer)
        fixed (void* __p_x = &x)
        fixed (void* __p_y = &y)
        fixed (void* __p_comp = &comp)
        {
            var __result = __fp(__p_buffer, len, __p_x, __p_y, __p_comp, reqComp);
            return __result;
        }
    }
}
