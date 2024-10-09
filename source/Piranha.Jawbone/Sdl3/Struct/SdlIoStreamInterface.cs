namespace Piranha.Jawbone.Sdl3;

public struct SdlIoStreamInterface // SDL_IOStreamInterface
{
    public uint Version; // Uint32 version
    public nint Size; // Sint64 (*)(void * userdata) * size
    public nint Seek; // Sint64 (*)(void * userdata, Sint64 offset, SDL_IOWhence whence) * seek
    public nint Read; // size_t (*)(void * userdata, void * ptr, size_t size, SDL_IOStatus * status) * read
    public nint Write; // size_t (*)(void * userdata, void const * ptr, size_t size, SDL_IOStatus * status) * write
    public nint Flush; // bool (*)(void * userdata, SDL_IOStatus * status) * flush
    public nint Close; // bool (*)(void * userdata) * close
}
