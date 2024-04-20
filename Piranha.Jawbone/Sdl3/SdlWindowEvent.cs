using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

// SDL_video.h -- SDL_WindowEventID
[StructLayout(LayoutKind.Sequential)]
public struct SdlWindowEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public int Data1;
    public int Data2;

    public readonly int X => Data1;
    public readonly int Y => Data2;
}
