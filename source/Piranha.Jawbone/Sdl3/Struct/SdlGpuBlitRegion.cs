namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuBlitRegion // SDL_GPUBlitRegion
{
    public nint Texture; // SDL_GPUTexture * texture
    public uint MipLevel; // Uint32 mip_level
    public uint LayerOrDepthPlane; // Uint32 layer_or_depth_plane
    public uint X; // Uint32 x
    public uint Y; // Uint32 y
    public uint W; // Uint32 w
    public uint H; // Uint32 h
}
