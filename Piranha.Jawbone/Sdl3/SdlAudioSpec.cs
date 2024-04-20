using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

// https://wiki.libsdl.org/SDL_AudioSpec

[StructLayout(LayoutKind.Sequential)]
public struct SdlAudioSpec
{
    public SdlAudioFormat Format;
    public int Channels;
    public int Freq;
}
