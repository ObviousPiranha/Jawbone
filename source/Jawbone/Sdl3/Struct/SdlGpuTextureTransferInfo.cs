namespace Jawbone.Sdl3;

public struct SdlGpuTextureTransferInfo // SDL_GPUTextureTransferInfo
{
    public nint TransferBuffer; // SDL_GPUTransferBuffer * transfer_buffer
    public uint Offset; // Uint32 offset
    public uint PixelsPerRow; // Uint32 pixels_per_row
    public uint RowsPerLayer; // Uint32 rows_per_layer
}
