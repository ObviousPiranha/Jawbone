namespace Piranha.Jawbone.Sdl3;

public enum SdlGpuStencilOp // SDL_GPUStencilOp
{
    Invalid = 0, // SDL_GPU_STENCILOP_INVALID
    Keep = 1, // SDL_GPU_STENCILOP_KEEP
    Zero = 2, // SDL_GPU_STENCILOP_ZERO
    Replace = 3, // SDL_GPU_STENCILOP_REPLACE
    IncrementAndClamp = 4, // SDL_GPU_STENCILOP_INCREMENT_AND_CLAMP
    DecrementAndClamp = 5, // SDL_GPU_STENCILOP_DECREMENT_AND_CLAMP
    Invert = 6, // SDL_GPU_STENCILOP_INVERT
    IncrementAndWrap = 7, // SDL_GPU_STENCILOP_INCREMENT_AND_WRAP
    DecrementAndWrap = 8, // SDL_GPU_STENCILOP_DECREMENT_AND_WRAP
}
