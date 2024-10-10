using System;

namespace Piranha.Jawbone.Sdl3;

public interface IAudioManager
{
    bool IsPaused { get; set; }
    float Gain { get; set; }

    int PrepareAudio(
        int frequency,
        int channels,
        ReadOnlySpan<short> data);

    int PrepareAudio(
        int frequency,
        int channels,
        ReadOnlySpan<float> data);

    int PlayAudio(int soundId);
}
