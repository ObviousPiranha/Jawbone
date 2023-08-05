using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

// SDL_video.h -- SDL_WindowEventID
[StructLayout(LayoutKind.Sequential)]
public struct SdlWindowEvent
{
    public const int Shown = 0x1;
    public const int Hidden = 0x2;
    public const int Exposed = 0x3;
    public const int Moved = 0x4;
    public const int Resized = 0x5;
    public const int SizeChanged = 0x6;
    public const int Minimized = 0x7;
    public const int Maximized = 0x8;
    public const int Restored = 0x9;
    public const int Enter = 0xa;
    public const int Leave = 0xb;
    public const int FocusGained = 0xc;
    public const int FocusLost = 0xd;
    public const int Close = 0xe;
    public const int TakeFocus = 0xf;
    public const int HitTest = 0x10;

    public uint Type;
    public uint Timestamp;
    public uint WindowId;
    public byte Event;
    public byte Padding1;
    public byte Padding2;
    public byte Padding3;
    public int Data1;
    public int Data2;

    public int X => Data1;
    public int Y => Data2;
}
