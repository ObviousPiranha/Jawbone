namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuShaderCreateInfo // SDL_GPUShaderCreateInfo
{
    public nuint CodeSize; // size_t code_size
    public nint Code; // Uint8 const * code
    public nint Entrypoint; // char const * entrypoint
    public uint Format; // SDL_GPUShaderFormat format
    public SdlGpuShaderStage Stage; // SDL_GPUShaderStage stage
    public uint NumSamplers; // Uint32 num_samplers
    public uint NumStorageTextures; // Uint32 num_storage_textures
    public uint NumStorageBuffers; // Uint32 num_storage_buffers
    public uint NumUniformBuffers; // Uint32 num_uniform_buffers
    public uint Props; // SDL_PropertiesID props
}
