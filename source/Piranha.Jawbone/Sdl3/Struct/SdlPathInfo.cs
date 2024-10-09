namespace Piranha.Jawbone.Sdl3;

public struct SdlPathInfo // SDL_PathInfo
{
    public SdlPathType Type; // SDL_PathType type
    public ulong Size; // Uint64 size
    public long CreateTime; // SDL_Time create_time
    public long ModifyTime; // SDL_Time modify_time
    public long AccessTime; // SDL_Time access_time
}
