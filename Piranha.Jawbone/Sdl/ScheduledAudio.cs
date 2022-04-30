namespace Piranha.Jawbone.Sdl;

readonly struct ScheduledAudio
{
    public long StartSampleIndex { get; init; }
    public float[] Samples { get; init; }
    public int LoopDelaySampleCount { get; init; }
    public int Id { get; init; }
}
