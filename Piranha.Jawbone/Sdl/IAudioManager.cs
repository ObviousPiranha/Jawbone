using System;

namespace Piranha.Jawbone.Sdl;

public interface IAudioManager
{
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
