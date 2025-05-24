namespace Jawbone.Sdl3;

public struct SdlGpuStencilOpState // SDL_GPUStencilOpState
{
    public SdlGpuStencilOp FailOp; // SDL_GPUStencilOp fail_op
    public SdlGpuStencilOp PassOp; // SDL_GPUStencilOp pass_op
    public SdlGpuStencilOp DepthFailOp; // SDL_GPUStencilOp depth_fail_op
    public SdlGpuCompareOp CompareOp; // SDL_GPUCompareOp compare_op
}
