// Code generated at 2025-06-21T19:03:14.

namespace Jawbone;

public sealed unsafe class NativePiranha
{
    private readonly nint _fp_GetNull;

    public NativePiranha(
        System.Func<string, nint> loader)
    {
        _fp_GetNull = loader.Invoke(nameof(GetNull));
    }

    public nint GetNull()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint
            >)_fp_GetNull;
        var __result = __fp();
        return __result;
    }
}
