using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlKeyboardEvent
{
    public uint Type;
    public uint Timestamp;
    public uint WindowId;
    public byte State;
    public byte Repeat;
    public byte Padding2;
    public byte Padding3;
    public SdlKeysym Keysym;
}
