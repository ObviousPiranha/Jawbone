namespace Jawbone.Sdl3;

public struct SdlGpuMultisampleState // SDL_GPUMultisampleState
{
    public SdlGpuSampleCount SampleCount; // SDL_GPUSampleCount sample_count
    public uint SampleMask; // Uint32 sample_mask
    public byte EnableMask; // bool enable_mask
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
