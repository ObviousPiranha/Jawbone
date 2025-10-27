namespace Jawbone.Sdl3;

public static class SdlGpuTextureUsage
{
    public const uint Sampler = 1 << 0;
    public const uint ColorTarget = 1 << 1;
    public const uint DepthStencilTarget = 1 << 2;
    public const uint GraphicsStorageRead = 1 << 3;
    public const uint ComputeStorageRead = 1 << 4;
    public const uint ComputeStorageWrite = 1 << 5;
    public const uint ComputeStorageSimultaneousReadWrite = 1 << 6;
}