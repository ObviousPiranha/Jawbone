using System;

namespace Piranha.Jawbone.Sdl2;

public delegate void AudioShader(int frequency, int channels, Span<float> samples);

public interface IAudioManager
{
    bool IsPaused { get; set; }
    void AddShader(AudioShader audioShader);
    void RemoveShader(AudioShader audioShader);

    int PrepareAudio(
        SdlAudioFormat format,
        int frequency,
        int channels,
        ReadOnlySpan<byte> data);

    int ScheduleAudio(int soundId, TimeSpan delay);
    int ScheduleLoopingAudio(
        int soundId,
        TimeSpan delay,
        TimeSpan delayBetweenLoops);

    bool CancelAudio(int scheduledAudioId);

    void PumpAudio();
}
