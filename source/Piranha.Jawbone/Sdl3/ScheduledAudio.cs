namespace Piranha.Jawbone.Sdl3;

readonly struct ScheduledAudio
{
    public long StartSampleIndex { get; init; }
    public float[] Samples { get; init; }
    public int LoopDelaySampleCount { get; init; }
    public int Id { get; init; }
}
