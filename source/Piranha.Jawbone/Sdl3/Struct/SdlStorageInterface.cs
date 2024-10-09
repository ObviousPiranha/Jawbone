namespace Piranha.Jawbone.Sdl3;

public struct SdlStorageInterface // SDL_StorageInterface
{
    public uint Version; // Uint32 version
    public nint Close; // bool (*)(void * userdata) * close
    public nint Ready; // bool (*)(void * userdata) * ready
    public nint Enumerate; // bool (*)(void * userdata, char const * path, SDL_EnumerateDirectoryCallback callback, void * callback_userdata) * enumerate
    public nint Info; // bool (*)(void * userdata, char const * path, SDL_PathInfo * info) * info
    public nint ReadFile; // bool (*)(void * userdata, char const * path, void * destination, Uint64 length) * read_file
    public nint WriteFile; // bool (*)(void * userdata, char const * path, void const * source, Uint64 length) * write_file
    public nint Mkdir; // bool (*)(void * userdata, char const * path) * mkdir
    public nint Remove; // bool (*)(void * userdata, char const * path) * remove
    public nint Rename; // bool (*)(void * userdata, char const * oldpath, char const * newpath) * rename
    public nint Copy; // bool (*)(void * userdata, char const * oldpath, char const * newpath) * copy
    public nint SpaceRemaining; // Uint64 (*)(void * userdata) * space_remaining
}
