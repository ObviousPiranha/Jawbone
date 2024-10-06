namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuVertexBufferDescription // SDL_GPUVertexBufferDescription
{
    public uint Slot; // Uint32 slot
    public uint Pitch; // Uint32 pitch
    public SdlGpuVertexInputRate InputRate; // SDL_GPUVertexInputRate input_rate
    public uint InstanceStepRate; // Uint32 instance_step_rate
}
