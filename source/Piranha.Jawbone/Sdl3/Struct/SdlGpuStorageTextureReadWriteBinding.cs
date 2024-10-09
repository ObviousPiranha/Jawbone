namespace Piranha.Jawbone.Sdl3;

public struct SdlGpuStorageTextureReadWriteBinding // SDL_GPUStorageTextureReadWriteBinding
{
    public nint Texture; // SDL_GPUTexture * texture
    public uint MipLevel; // Uint32 mip_level
    public uint Layer; // Uint32 layer
    public byte Cycle; // bool cycle
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
