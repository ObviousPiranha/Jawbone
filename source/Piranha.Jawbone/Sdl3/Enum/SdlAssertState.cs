namespace Piranha.Jawbone.Sdl3;

public enum SdlAssertState // SDL_AssertState
{
    Retry = 0, // SDL_ASSERTION_RETRY
    Break = 1, // SDL_ASSERTION_BREAK
    Abort = 2, // SDL_ASSERTION_ABORT
    Ignore = 3, // SDL_ASSERTION_IGNORE
    AlwaysIgnore = 4, // SDL_ASSERTION_ALWAYS_IGNORE
}
