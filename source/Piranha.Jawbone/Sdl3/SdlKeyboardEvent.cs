using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Sequential)]
public struct SdlKeyboardEvent
{
    public SdlEventType Type;
    public uint Reserved;
    public ulong Timestamp;
    public uint WindowId;
    public uint KeyboardId;
    public byte State;
    public byte Repeat;
    public byte Padding2;
    public byte Padding3;
    public SdlKeysym Keysym;
}
