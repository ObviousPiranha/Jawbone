namespace Jawbone.Sdl3;

public struct SdlGpuTextureCreateInfo // SDL_GPUTextureCreateInfo
{
    public SdlGpuTextureType Type; // SDL_GPUTextureType type
    public SdlGpuTextureFormat Format; // SDL_GPUTextureFormat format
    public uint Usage; // SDL_GPUTextureUsageFlags usage
    public uint Width; // Uint32 width
    public uint Height; // Uint32 height
    public uint LayerCountOrDepth; // Uint32 layer_count_or_depth
    public uint NumLevels; // Uint32 num_levels
    public SdlGpuSampleCount SampleCount; // SDL_GPUSampleCount sample_count
    public uint Props; // SDL_PropertiesID props
}
