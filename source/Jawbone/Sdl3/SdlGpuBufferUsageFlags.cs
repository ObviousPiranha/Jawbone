namespace Jawbone.Sdl3;

public static class SdlGpuBufferUsageFlags
{
    public const uint Vertex = 1 << 0;
    public const uint Index = 1 << 1;
    public const uint Indirect = 1 << 2;
    public const uint GraphicsStorageRead = 1 << 3;
    public const uint ComputeStorageRead = 1 << 4;
    public const uint ComputeStorageWrite = 1 << 5;
}
