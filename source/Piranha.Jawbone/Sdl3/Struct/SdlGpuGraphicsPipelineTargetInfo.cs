namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuGraphicsPipelineTargetInfo // SDL_GPUGraphicsPipelineTargetInfo
{
    public nint ColorTargetDescriptions; // SDL_GPUColorTargetDescription const * color_target_descriptions
    public uint NumColorTargets; // Uint32 num_color_targets
    public SdlGpuTextureFormat DepthStencilFormat; // SDL_GPUTextureFormat depth_stencil_format
    public byte HasDepthStencilTarget; // bool has_depth_stencil_target
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
