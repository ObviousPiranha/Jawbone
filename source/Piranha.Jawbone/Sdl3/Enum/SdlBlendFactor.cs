namespace Piranha.Jawbone.Sdl3;

public enum SdlBlendFactor // SDL_BlendFactor
{
    Zero = 1, // SDL_BLENDFACTOR_ZERO
    One = 2, // SDL_BLENDFACTOR_ONE
    SrcColor = 3, // SDL_BLENDFACTOR_SRC_COLOR
    OneMinusSrcColor = 4, // SDL_BLENDFACTOR_ONE_MINUS_SRC_COLOR
    SrcAlpha = 5, // SDL_BLENDFACTOR_SRC_ALPHA
    OneMinusSrcAlpha = 6, // SDL_BLENDFACTOR_ONE_MINUS_SRC_ALPHA
    DstColor = 7, // SDL_BLENDFACTOR_DST_COLOR
    OneMinusDstColor = 8, // SDL_BLENDFACTOR_ONE_MINUS_DST_COLOR
    DstAlpha = 9, // SDL_BLENDFACTOR_DST_ALPHA
    OneMinusDstAlpha = 10, // SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA
}
