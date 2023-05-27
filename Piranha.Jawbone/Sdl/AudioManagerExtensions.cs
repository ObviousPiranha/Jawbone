using Piranha.Jawbone.Tools.CollectionExtensions;
using System;

namespace Piranha.Jawbone.Sdl;

public static class AudioManagerExtensions
{
    public static int PrepareAudio(
        this IAudioManager audioManager,
        int frequency,
        int channels,
        ReadOnlySpan<float> f32Data)
    {
        return audioManager.PrepareAudio(
            SdlAudio.F32,
            frequency,
            channels,
            f32Data.ToByteSpan());
    }

    public static int PrepareAudio(
        this IAudioManager audioManager,
        int frequency,
        int channels,
        ReadOnlySpan<short> s16Data)
    {
        return audioManager.PrepareAudio(
            SdlAudio.S16Lsb,
            frequency,
            channels,
            s16Data.ToByteSpan());
    }
}
