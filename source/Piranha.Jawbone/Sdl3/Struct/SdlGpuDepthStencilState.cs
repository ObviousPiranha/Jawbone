namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuDepthStencilState // SDL_GPUDepthStencilState
{
    public SdlGpuCompareOp CompareOp; // SDL_GPUCompareOp compare_op
    public nint BackStencilState; // SDL_GPUStencilOpState back_stencil_state
    public nint FrontStencilState; // SDL_GPUStencilOpState front_stencil_state
    public byte CompareMask; // Uint8 compare_mask
    public byte WriteMask; // Uint8 write_mask
    public byte EnableDepthTest; // bool enable_depth_test
    public byte EnableDepthWrite; // bool enable_depth_write
    public byte EnableStencilTest; // bool enable_stencil_test
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
