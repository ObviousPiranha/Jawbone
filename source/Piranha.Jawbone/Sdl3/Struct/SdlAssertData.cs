namespace Piranha.Jawbone.Sdl3;

public struct SdlAssertData // SDL_AssertData
{
    public byte AlwaysIgnore; // bool always_ignore
    public uint TriggerCount; // unsigned int trigger_count
    public nint Condition; // char const * condition
    public nint Filename; // char const * filename
    public int Linenum; // int linenum
    public nint Function; // char const * function
    public nint Next; // SDL_AssertData const * next
}
