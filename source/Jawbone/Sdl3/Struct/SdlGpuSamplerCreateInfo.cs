namespace Jawbone.Sdl3;

public struct SdlGpuSamplerCreateInfo // SDL_GPUSamplerCreateInfo
{
    public SdlGpuFilter MinFilter; // SDL_GPUFilter min_filter
    public SdlGpuFilter MagFilter; // SDL_GPUFilter mag_filter
    public SdlGpuSamplerMipmapMode MipmapMode; // SDL_GPUSamplerMipmapMode mipmap_mode
    public SdlGpuSamplerAddressMode AddressModeU; // SDL_GPUSamplerAddressMode address_mode_u
    public SdlGpuSamplerAddressMode AddressModeV; // SDL_GPUSamplerAddressMode address_mode_v
    public SdlGpuSamplerAddressMode AddressModeW; // SDL_GPUSamplerAddressMode address_mode_w
    public float MipLodBias; // float mip_lod_bias
    public float MaxAnisotropy; // float max_anisotropy
    public SdlGpuCompareOp CompareOp; // SDL_GPUCompareOp compare_op
    public float MinLod; // float min_lod
    public float MaxLod; // float max_lod
    public byte EnableAnisotropy; // bool enable_anisotropy
    public byte EnableCompare; // bool enable_compare
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public uint Props; // SDL_PropertiesID props
}
