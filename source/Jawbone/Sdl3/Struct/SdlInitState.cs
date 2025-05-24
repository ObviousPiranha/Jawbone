namespace Jawbone.Sdl3;

public struct SdlInitState // SDL_InitState
{
    public nint Status; // SDL_AtomicInt status
    public ulong Thread; // SDL_ThreadID thread
    public nint Reserved; // void * reserved
}
