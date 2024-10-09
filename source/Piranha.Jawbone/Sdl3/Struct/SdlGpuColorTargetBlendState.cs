namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuColorTargetBlendState // SDL_GPUColorTargetBlendState
{
    public SdlGpuBlendFactor SrcColorBlendfactor; // SDL_GPUBlendFactor src_color_blendfactor
    public SdlGpuBlendFactor DstColorBlendfactor; // SDL_GPUBlendFactor dst_color_blendfactor
    public SdlGpuBlendOp ColorBlendOp; // SDL_GPUBlendOp color_blend_op
    public SdlGpuBlendFactor SrcAlphaBlendfactor; // SDL_GPUBlendFactor src_alpha_blendfactor
    public SdlGpuBlendFactor DstAlphaBlendfactor; // SDL_GPUBlendFactor dst_alpha_blendfactor
    public SdlGpuBlendOp AlphaBlendOp; // SDL_GPUBlendOp alpha_blend_op
    public byte ColorWriteMask; // SDL_GPUColorComponentFlags color_write_mask
    public byte EnableBlend; // bool enable_blend
    public byte EnableColorWriteMask; // bool enable_color_write_mask
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
}
