using System;

namespace Piranha.Jawbone.Opus;

public sealed class OpusDecoder : IDisposable
{
    private readonly IOpus _opus;
    private readonly IntPtr _decoder;
    private bool _destroyed;

    public int SamplingRate { get; }
    public int Channels { get; }

    public OpusDecoder(IOpus opus) : this(opus, 48000, 2)
    {
    }
    
    public OpusDecoder(IOpus opus, int samplingRate, int channels)
    {
        SamplingRate = samplingRate;
        Channels = channels;

        _opus = opus;
        _decoder = _opus.DecoderCreate(samplingRate, channels, out var error);

        OpusException.ThrowOnError(error);
    }

    ~OpusDecoder() => Destroy();

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    private void Destroy()
    {
        if (!_destroyed)
        {
            _opus.DecoderDestroy(_decoder);
            _destroyed = true;
        }
    }

    private void EnsureNotDestroyed()
    {
        if (_destroyed)
            throw new ObjectDisposedException(nameof(OpusEncoder));
    }

    public int Decode(ReadOnlySpan<byte> data, Span<short> pcm)
    {
        var frameSize = pcm.Length / Channels;
        var length =_opus.Decode(
            _decoder,
            data[0],
            data.Length,
            out pcm[0],
            frameSize,
            0);
        OpusException.ThrowOnError(length);
        return length;
    }

    public int Decode(ReadOnlySpan<byte> data, Span<float> pcm)
    {
        var frameSize = pcm.Length / Channels;
        var length = _opus.DecodeFloat(
            _decoder,
            data[0],
            data.Length,
            out pcm[0],
            frameSize,
            1);
        OpusException.ThrowOnError(length);
        return length;
    }
}