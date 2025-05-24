namespace Jawbone.Sdl3;

public struct SdlUserEvent // SDL_UserEvent
{
    public uint Type; // Uint32 type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public int Code; // Sint32 code
    public nint Data1; // void * data1
    public nint Data2; // void * data2
}
