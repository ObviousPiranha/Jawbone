using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

// https://wiki.libsdl.org/SDL_AudioSpec

[StructLayout(LayoutKind.Sequential)]
public struct AudioSpec
{
    public int freq;
    public ushort format;
    public byte channels;
    public byte silence;
    public ushort samples;
    public uint size;
    public IntPtr callback;
    public IntPtr userdata;
}
