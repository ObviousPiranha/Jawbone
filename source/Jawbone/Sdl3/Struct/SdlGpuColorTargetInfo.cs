namespace Jawbone.Sdl3;

public struct SdlGpuColorTargetInfo // SDL_GPUColorTargetInfo
{
    public nint Texture; // SDL_GPUTexture * texture
    public uint MipLevel; // Uint32 mip_level
    public uint LayerOrDepthPlane; // Uint32 layer_or_depth_plane
    public SdlFColor ClearColor; // SDL_FColor clear_color
    public SdlGpuLoadOp LoadOp; // SDL_GPULoadOp load_op
    public SdlGpuStoreOp StoreOp; // SDL_GPUStoreOp store_op
    public nint ResolveTexture; // SDL_GPUTexture * resolve_texture
    public uint ResolveMipLevel; // Uint32 resolve_mip_level
    public uint ResolveLayer; // Uint32 resolve_layer
    public byte Cycle; // bool cycle
    public byte CycleResolveTexture; // bool cycle_resolve_texture
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
}
