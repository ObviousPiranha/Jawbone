using System.Collections.Generic;

namespace Jawbone.Sdl3;

public static class SdlGpuShaderFormat
{
    public const uint Invalid = 0;
    public const uint Private = 1 << 0;
    public const uint Spirv = 1 << 1;
    public const uint Dxbc = 1 << 2;
    public const uint Dxil = 1 << 3;
    public const uint Msl = 1 << 4;
    public const uint MetalLib = 1 << 5;

    public static IEnumerable<string> EnumerateFormatNames(uint value)
    {
        if (IsSet(Private))
            yield return nameof(Private);
        if (IsSet(Spirv))
            yield return nameof(Spirv);
        if (IsSet(Dxbc))
            yield return nameof(Dxbc);
        if (IsSet(Dxil))
            yield return nameof(Dxil);
        if (IsSet(Msl))
            yield return nameof(Msl);
        if (IsSet(MetalLib))
            yield return nameof(MetalLib);
        bool IsSet(uint item) => (value & item) != 0;
    }
}
