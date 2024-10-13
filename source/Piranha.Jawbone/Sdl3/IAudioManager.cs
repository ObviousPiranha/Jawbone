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

    int PlayAudio(int soundId, float gain = 1f, float ratio = 1f);
    bool TrySetGain(int playbackId, float gain);
    bool TrySetRatio(int playbackId, float ratio);
    bool TryStopAudio(int playbackId);
}
