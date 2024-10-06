namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuBufferRegion // SDL_GPUBufferRegion
{
    public nint Buffer; // SDL_GPUBuffer * buffer
    public uint Offset; // Uint32 offset
    public uint Size; // Uint32 size
}
