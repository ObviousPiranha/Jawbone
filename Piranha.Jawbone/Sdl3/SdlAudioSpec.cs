using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

// https://wiki.libsdl.org/SDL_AudioSpec

[StructLayout(LayoutKind.Sequential)]
public struct SdlAudioSpec
{
    public int Freq;
    public SdlAudioFormat Format;
    public byte Channels;
    public byte Silence;
    public ushort Samples;
    public ushort Padding;
    public uint Size;
    public IntPtr Callback;
    public IntPtr Userdata;
}
