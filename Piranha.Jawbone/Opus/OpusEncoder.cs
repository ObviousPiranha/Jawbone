using System;

namespace Piranha.Jawbone.Opus;

public sealed class OpusEncoder : IDisposable
{
    private readonly IOpus _opus;
    private readonly IntPtr _encoder;
    private bool _destroyed;

    public int SamplingRate { get; }
    public int Channels { get; }

    public OpusEncoder(IOpus opus) : this(opus, 48000, 2, OpusApplication.Audio)
    {
    }

    public OpusEncoder(IOpus opus, int samplingRate, int channels, OpusApplication application)
    {
        SamplingRate = samplingRate;
        Channels = channels;

        _opus = opus;
        _encoder = _opus.EncoderCreate(
            samplingRate,
            channels,
            (int)application,
            out var error);

        OpusException.ThrowOnError(error);
    }

    ~OpusEncoder() => Destroy();

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    private void Destroy()
    {
        if (!_destroyed)
        {
            _opus.EncoderDestroy(_encoder);
            _destroyed = true;
        }
    }

    private void EnsureNotDestroyed()
    {
        if (_destroyed)
            throw new ObjectDisposedException(nameof(OpusEncoder));
    }

    public int Encode(ReadOnlySpan<float> pcm, Span<byte> packet)
    {
        EnsureNotDestroyed();

        // https://opus-codec.org/docs/opus_api-1.3.1/group__opus__encoder.html#ga4ae9905859cd241ef4bb5c59cd5e5309
        // For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880.
        var frameSize = pcm.Length / Channels;
        var length = _opus.EncodeFloat(
            _encoder,
            pcm[0],
            frameSize,
            out packet[0],
            packet.Length);
        OpusException.ThrowOnError(length);
        return length;
    }

    public int Encode(ReadOnlySpan<short> pcm, Span<byte> packet)
    {
        EnsureNotDestroyed();
        var frameSize = pcm.Length / Channels;
        var length = _opus.Encode(
            _encoder,
            pcm[0],
            frameSize,
            out packet[0],
            packet.Length);
        OpusException.ThrowOnError(length);
        return length;
    }
}