namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuRasterizerState // SDL_GPURasterizerState
{
    public SdlGpuFillMode FillMode; // SDL_GPUFillMode fill_mode
    public SdlGpuCullMode CullMode; // SDL_GPUCullMode cull_mode
    public SdlGpuFrontFace FrontFace; // SDL_GPUFrontFace front_face
    public float DepthBiasConstantFactor; // float depth_bias_constant_factor
    public float DepthBiasClamp; // float depth_bias_clamp
    public float DepthBiasSlopeFactor; // float depth_bias_slope_factor
    public byte EnableDepthBias; // bool enable_depth_bias
    public byte EnableDepthClip; // bool enable_depth_clip
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
}
