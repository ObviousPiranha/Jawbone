namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuTextureLocation // SDL_GPUTextureLocation
{
    public nint Texture; // SDL_GPUTexture * texture
    public uint MipLevel; // Uint32 mip_level
    public uint Layer; // Uint32 layer
    public uint X; // Uint32 x
    public uint Y; // Uint32 y
    public uint Z; // Uint32 z
}
