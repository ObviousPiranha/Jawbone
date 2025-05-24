using System;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlKeyboardEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public byte State;
    public byte Repeat;
    public byte Padding2;
    public byte Padding3;
    public SdlKeysym Keysym;
}
