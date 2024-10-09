namespace Piranha.Jawbone.Sdl3;

public enum SdlIoStatus // SDL_IOStatus
{
    Ready = 0, // SDL_IO_STATUS_READY
    Error = 1, // SDL_IO_STATUS_ERROR
    Eof = 2, // SDL_IO_STATUS_EOF
    NotReady = 3, // SDL_IO_STATUS_NOT_READY
    Readonly = 4, // SDL_IO_STATUS_READONLY
    Writeonly = 5, // SDL_IO_STATUS_WRITEONLY
}
