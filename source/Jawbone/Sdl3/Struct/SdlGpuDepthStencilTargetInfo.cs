namespace Jawbone.Sdl3;

public struct SdlGpuDepthStencilTargetInfo // SDL_GPUDepthStencilTargetInfo
{
    public nint Texture; // SDL_GPUTexture * texture
    public float ClearDepth; // float clear_depth
    public SdlGpuLoadOp LoadOp; // SDL_GPULoadOp load_op
    public SdlGpuStoreOp StoreOp; // SDL_GPUStoreOp store_op
    public SdlGpuLoadOp StencilLoadOp; // SDL_GPULoadOp stencil_load_op
    public SdlGpuStoreOp StencilStoreOp; // SDL_GPUStoreOp stencil_store_op
    public byte Cycle; // bool cycle
    public byte ClearStencil; // Uint8 clear_stencil
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
}
