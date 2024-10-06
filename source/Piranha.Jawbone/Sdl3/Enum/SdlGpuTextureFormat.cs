namespace Piranha.Jawbone.Sdl3;

public enum SdlGpuTextureFormat // SDL_GPUTextureFormat
{
    Invalid = 0, // SDL_GPU_TEXTUREFORMAT_INVALID
    A8Unorm = 1, // SDL_GPU_TEXTUREFORMAT_A8_UNORM
    R8Unorm = 2, // SDL_GPU_TEXTUREFORMAT_R8_UNORM
    R8G8Unorm = 3, // SDL_GPU_TEXTUREFORMAT_R8G8_UNORM
    R8G8B8A8Unorm = 4, // SDL_GPU_TEXTUREFORMAT_R8G8B8A8_UNORM
    R16Unorm = 5, // SDL_GPU_TEXTUREFORMAT_R16_UNORM
    R16G16Unorm = 6, // SDL_GPU_TEXTUREFORMAT_R16G16_UNORM
    R16G16B16A16Unorm = 7, // SDL_GPU_TEXTUREFORMAT_R16G16B16A16_UNORM
    R10G10B10A2Unorm = 8, // SDL_GPU_TEXTUREFORMAT_R10G10B10A2_UNORM
    B5G6R5Unorm = 9, // SDL_GPU_TEXTUREFORMAT_B5G6R5_UNORM
    B5G5R5A1Unorm = 10, // SDL_GPU_TEXTUREFORMAT_B5G5R5A1_UNORM
    B4G4R4A4Unorm = 11, // SDL_GPU_TEXTUREFORMAT_B4G4R4A4_UNORM
    B8G8R8A8Unorm = 12, // SDL_GPU_TEXTUREFORMAT_B8G8R8A8_UNORM
    Bc1RgbaUnorm = 13, // SDL_GPU_TEXTUREFORMAT_BC1_RGBA_UNORM
    Bc2RgbaUnorm = 14, // SDL_GPU_TEXTUREFORMAT_BC2_RGBA_UNORM
    Bc3RgbaUnorm = 15, // SDL_GPU_TEXTUREFORMAT_BC3_RGBA_UNORM
    Bc4RUnorm = 16, // SDL_GPU_TEXTUREFORMAT_BC4_R_UNORM
    Bc5RgUnorm = 17, // SDL_GPU_TEXTUREFORMAT_BC5_RG_UNORM
    Bc7RgbaUnorm = 18, // SDL_GPU_TEXTUREFORMAT_BC7_RGBA_UNORM
    Bc6HRgbFloat = 19, // SDL_GPU_TEXTUREFORMAT_BC6H_RGB_FLOAT
    Bc6HRgbUfloat = 20, // SDL_GPU_TEXTUREFORMAT_BC6H_RGB_UFLOAT
    R8Snorm = 21, // SDL_GPU_TEXTUREFORMAT_R8_SNORM
    R8G8Snorm = 22, // SDL_GPU_TEXTUREFORMAT_R8G8_SNORM
    R8G8B8A8Snorm = 23, // SDL_GPU_TEXTUREFORMAT_R8G8B8A8_SNORM
    R16Snorm = 24, // SDL_GPU_TEXTUREFORMAT_R16_SNORM
    R16G16Snorm = 25, // SDL_GPU_TEXTUREFORMAT_R16G16_SNORM
    R16G16B16A16Snorm = 26, // SDL_GPU_TEXTUREFORMAT_R16G16B16A16_SNORM
    R16Float = 27, // SDL_GPU_TEXTUREFORMAT_R16_FLOAT
    R16G16Float = 28, // SDL_GPU_TEXTUREFORMAT_R16G16_FLOAT
    R16G16B16A16Float = 29, // SDL_GPU_TEXTUREFORMAT_R16G16B16A16_FLOAT
    R32Float = 30, // SDL_GPU_TEXTUREFORMAT_R32_FLOAT
    R32G32Float = 31, // SDL_GPU_TEXTUREFORMAT_R32G32_FLOAT
    R32G32B32A32Float = 32, // SDL_GPU_TEXTUREFORMAT_R32G32B32A32_FLOAT
    R11G11B10Ufloat = 33, // SDL_GPU_TEXTUREFORMAT_R11G11B10_UFLOAT
    R8Uint = 34, // SDL_GPU_TEXTUREFORMAT_R8_UINT
    R8G8Uint = 35, // SDL_GPU_TEXTUREFORMAT_R8G8_UINT
    R8G8B8A8Uint = 36, // SDL_GPU_TEXTUREFORMAT_R8G8B8A8_UINT
    R16Uint = 37, // SDL_GPU_TEXTUREFORMAT_R16_UINT
    R16G16Uint = 38, // SDL_GPU_TEXTUREFORMAT_R16G16_UINT
    R16G16B16A16Uint = 39, // SDL_GPU_TEXTUREFORMAT_R16G16B16A16_UINT
    R32Uint = 40, // SDL_GPU_TEXTUREFORMAT_R32_UINT
    R32G32Uint = 41, // SDL_GPU_TEXTUREFORMAT_R32G32_UINT
    R32G32B32A32Uint = 42, // SDL_GPU_TEXTUREFORMAT_R32G32B32A32_UINT
    R8Int = 43, // SDL_GPU_TEXTUREFORMAT_R8_INT
    R8G8Int = 44, // SDL_GPU_TEXTUREFORMAT_R8G8_INT
    R8G8B8A8Int = 45, // SDL_GPU_TEXTUREFORMAT_R8G8B8A8_INT
    R16Int = 46, // SDL_GPU_TEXTUREFORMAT_R16_INT
    R16G16Int = 47, // SDL_GPU_TEXTUREFORMAT_R16G16_INT
    R16G16B16A16Int = 48, // SDL_GPU_TEXTUREFORMAT_R16G16B16A16_INT
    R32Int = 49, // SDL_GPU_TEXTUREFORMAT_R32_INT
    R32G32Int = 50, // SDL_GPU_TEXTUREFORMAT_R32G32_INT
    R32G32B32A32Int = 51, // SDL_GPU_TEXTUREFORMAT_R32G32B32A32_INT
    R8G8B8A8UnormSrgb = 52, // SDL_GPU_TEXTUREFORMAT_R8G8B8A8_UNORM_SRGB
    B8G8R8A8UnormSrgb = 53, // SDL_GPU_TEXTUREFORMAT_B8G8R8A8_UNORM_SRGB
    Bc1RgbaUnormSrgb = 54, // SDL_GPU_TEXTUREFORMAT_BC1_RGBA_UNORM_SRGB
    Bc2RgbaUnormSrgb = 55, // SDL_GPU_TEXTUREFORMAT_BC2_RGBA_UNORM_SRGB
    Bc3RgbaUnormSrgb = 56, // SDL_GPU_TEXTUREFORMAT_BC3_RGBA_UNORM_SRGB
    Bc7RgbaUnormSrgb = 57, // SDL_GPU_TEXTUREFORMAT_BC7_RGBA_UNORM_SRGB
    D16Unorm = 58, // SDL_GPU_TEXTUREFORMAT_D16_UNORM
    D24Unorm = 59, // SDL_GPU_TEXTUREFORMAT_D24_UNORM
    D32Float = 60, // SDL_GPU_TEXTUREFORMAT_D32_FLOAT
    D24UnormS8Uint = 61, // SDL_GPU_TEXTUREFORMAT_D24_UNORM_S8_UINT
    D32FloatS8Uint = 62, // SDL_GPU_TEXTUREFORMAT_D32_FLOAT_S8_UINT
}