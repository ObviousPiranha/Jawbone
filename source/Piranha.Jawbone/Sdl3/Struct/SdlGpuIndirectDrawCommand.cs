namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuIndirectDrawCommand // SDL_GPUIndirectDrawCommand
{
    public uint NumVertices; // Uint32 num_vertices
    public uint NumInstances; // Uint32 num_instances
    public uint FirstVertex; // Uint32 first_vertex
    public uint FirstInstance; // Uint32 first_instance
}
