namespace Jawbone.Sdl3;

public struct SdlPixelFormatDetails // SDL_PixelFormatDetails
{
    public SdlPixelFormat Format; // SDL_PixelFormat format
    public byte BitsPerPixel; // Uint8 bits_per_pixel
    public byte BytesPerPixel; // Uint8 bytes_per_pixel
    public nint Padding; // Uint8[2] padding
    public uint Rmask; // Uint32 Rmask
    public uint Gmask; // Uint32 Gmask
    public uint Bmask; // Uint32 Bmask
    public uint Amask; // Uint32 Amask
    public byte Rbits; // Uint8 Rbits
    public byte Gbits; // Uint8 Gbits
    public byte Bbits; // Uint8 Bbits
    public byte Abits; // Uint8 Abits
    public byte Rshift; // Uint8 Rshift
    public byte Gshift; // Uint8 Gshift
    public byte Bshift; // Uint8 Bshift
    public byte Ashift; // Uint8 Ashift
}
