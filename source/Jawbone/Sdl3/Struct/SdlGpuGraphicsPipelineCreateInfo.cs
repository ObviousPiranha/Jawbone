namespace Jawbone.Sdl3;

public struct SdlGpuGraphicsPipelineCreateInfo // SDL_GPUGraphicsPipelineCreateInfo
{
    public nint VertexShader; // SDL_GPUShader * vertex_shader
    public nint FragmentShader; // SDL_GPUShader * fragment_shader
    public SdlGpuVertexInputState VertexInputState; // SDL_GPUVertexInputState vertex_input_state
    public SdlGpuPrimitiveType PrimitiveType; // SDL_GPUPrimitiveType primitive_type
    public SdlGpuRasterizerState RasterizerState; // SDL_GPURasterizerState rasterizer_state
    public SdlGpuMultisampleState MultisampleState; // SDL_GPUMultisampleState multisample_state
    public SdlGpuDepthStencilState DepthStencilState; // SDL_GPUDepthStencilState depth_stencil_state
    public SdlGpuGraphicsPipelineTargetInfo TargetInfo; // SDL_GPUGraphicsPipelineTargetInfo target_info
    public uint Props; // SDL_PropertiesID props
}
