using Jawbone.Extensions;
using System;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

public static class AudioManagerExtensions
{
    public static int PrepareAudio(
        this IAudioManager audioManager,
        int frequency,
        int channels,
        ReadOnlySpan<float> f32Data)
    {
        return audioManager.PrepareAudio(
            SdlAudioFormat.F32,
            frequency,
            channels,
            MemoryMarshal.AsBytes(f32Data));
    }

    public static int PrepareAudio(
        this IAudioManager audioManager,
        int frequency,
        int channels,
        ReadOnlySpan<short> s16Data)
    {
        return audioManager.PrepareAudio(
            SdlAudioFormat.S16Lsb,
            frequency,
            channels,
            MemoryMarshal.AsBytes(s16Data));
    }
}
