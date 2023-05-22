using System;

namespace Piranha.Jawbone.Sdl;

public delegate void AudioShader(int frequency, int channels, Span<float> samples);

public interface IAudioManager
{
    void AddShader(AudioShader audioShader);
    void RemoveShader(AudioShader audioShader);

    int PrepareAudio(
        SdlAudio format,
        int frequency,
        int channels,
        ReadOnlySpan<byte> data);

    int ScheduleAudio(int soundId, TimeSpan delay);
    int ScheduleLoopingAudio(
        int soundId,
        TimeSpan delay,
        TimeSpan delayBetweenLoops);

    bool CancelAudio(int scheduledAudioId);
}
