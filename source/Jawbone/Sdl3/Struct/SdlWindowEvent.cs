namespace Jawbone.Sdl3;

public struct SdlWindowEvent // SDL_WindowEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public int Data1; // Sint32 data1
    public int Data2; // Sint32 data2

    public readonly int X => Data1;
    public readonly int Y => Data2;
}
