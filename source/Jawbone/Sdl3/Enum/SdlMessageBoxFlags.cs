namespace Jawbone.Sdl3;

public enum SdlMessageBoxFlags : uint
{
    None,
    Error = 1u << 4, // SDL_MESSAGEBOX_ERROR
    Warning = 1u << 5, // SDL_MESSAGEBOX_WARNING
    Information = 1u << 6, // SDL_MESSAGEBOX_INFORMATION
    ButtonsLeftToRight = 1u << 7, // SDL_MESSAGEBOX_BUTTONS_LEFT_TO_RIGHT
    ButtonsRightToLeft = 1u << 8 // SDL_MESSAGEBOX_BUTTONS_RIGHT_TO_LEFT
}
