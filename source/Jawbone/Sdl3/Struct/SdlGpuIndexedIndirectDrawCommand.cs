namespace Jawbone.Sdl3;

public struct SdlGpuIndexedIndirectDrawCommand // SDL_GPUIndexedIndirectDrawCommand
{
    public uint NumIndices; // Uint32 num_indices
    public uint NumInstances; // Uint32 num_instances
    public uint FirstIndex; // Uint32 first_index
    public int VertexOffset; // Sint32 vertex_offset
    public uint FirstInstance; // Uint32 first_instance
}
