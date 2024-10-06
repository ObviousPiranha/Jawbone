namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuGraphicsPipelineCreateInfo // SDL_GPUGraphicsPipelineCreateInfo
{
    public nint VertexShader; // SDL_GPUShader * vertex_shader
    public nint FragmentShader; // SDL_GPUShader * fragment_shader
    public nint VertexInputState; // SDL_GPUVertexInputState vertex_input_state
    public SdlGpuPrimitiveType PrimitiveType; // SDL_GPUPrimitiveType primitive_type
    public nint RasterizerState; // SDL_GPURasterizerState rasterizer_state
    public nint MultisampleState; // SDL_GPUMultisampleState multisample_state
    public nint DepthStencilState; // SDL_GPUDepthStencilState depth_stencil_state
    public nint TargetInfo; // SDL_GPUGraphicsPipelineTargetInfo target_info
    public uint Props; // SDL_PropertiesID props
}
