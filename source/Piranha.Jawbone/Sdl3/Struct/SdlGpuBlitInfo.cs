namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuBlitInfo // SDL_GPUBlitInfo
{
    public nint Source; // SDL_GPUBlitRegion source
    public nint Destination; // SDL_GPUBlitRegion destination
    public SdlGpuLoadOp LoadOp; // SDL_GPULoadOp load_op
    public nint ClearColor; // SDL_FColor clear_color
    public SdlFlipMode FlipMode; // SDL_FlipMode flip_mode
    public SdlGpuFilter Filter; // SDL_GPUFilter filter
    public byte Cycle; // bool cycle
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
