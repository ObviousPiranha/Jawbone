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
}