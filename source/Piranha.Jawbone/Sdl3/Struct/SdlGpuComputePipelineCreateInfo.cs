namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuComputePipelineCreateInfo // SDL_GPUComputePipelineCreateInfo
{
    public nuint CodeSize; // size_t code_size
    public nint Code; // Uint8 const * code
    public nint Entrypoint; // char const * entrypoint
    public uint Format; // SDL_GPUShaderFormat format
    public uint NumSamplers; // Uint32 num_samplers
    public uint NumReadonlyStorageTextures; // Uint32 num_readonly_storage_textures
    public uint NumReadonlyStorageBuffers; // Uint32 num_readonly_storage_buffers
    public uint NumReadwriteStorageTextures; // Uint32 num_readwrite_storage_textures
    public uint NumReadwriteStorageBuffers; // Uint32 num_readwrite_storage_buffers
    public uint NumUniformBuffers; // Uint32 num_uniform_buffers
    public uint ThreadcountX; // Uint32 threadcount_x
    public uint ThreadcountY; // Uint32 threadcount_y
    public uint ThreadcountZ; // Uint32 threadcount_z
    public uint Props; // SDL_PropertiesID props
}
