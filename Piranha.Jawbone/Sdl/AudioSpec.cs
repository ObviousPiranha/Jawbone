using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

// https://wiki.libsdl.org/SDL_AudioSpec

[StructLayout(LayoutKind.Sequential)]
public struct AudioSpec
{
    public int Freq;
    public ushort Format;
    public byte Channels;
    public byte Silence;
    public ushort Samples;
    public uint Size;
    public IntPtr Callback;
    public IntPtr Userdata;
}
