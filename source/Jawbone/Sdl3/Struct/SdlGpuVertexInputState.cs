namespace Jawbone.Sdl3;

public struct SdlGpuVertexInputState // SDL_GPUVertexInputState
{
    public nint VertexBufferDescriptions; // SDL_GPUVertexBufferDescription const * vertex_buffer_descriptions
    public uint NumVertexBuffers; // Uint32 num_vertex_buffers
    public nint VertexAttributes; // SDL_GPUVertexAttribute const * vertex_attributes
    public uint NumVertexAttributes; // Uint32 num_vertex_attributes
}
