using System;

namespace Piranha.Jawbone.Opus;

public sealed class OpusDecoder : IDisposable
{
    private readonly IOpus _opus;
    private readonly IntPtr _decoder;
    private bool _destroyed;

    public int SamplingRate { get; }
    public int ChannelCount { get; }
    
    internal OpusDecoder(IOpus opus, int sampleRate, int channelCount)
    {
        SamplingRate = sampleRate;
        ChannelCount = channelCount;

        _opus = opus;
        _decoder = _opus.DecoderCreate(sampleRate, channelCount, out var error);

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

    public int Decode(ReadOnlySpan<byte> data, Span<short> pcm, bool decodeFec = false)
    {
        var frameSize = pcm.Length / ChannelCount;
        var length =_opus.Decode(
            _decoder,
            data[0],
            data.Length,
            out pcm[0],
            frameSize,
            Convert.ToInt32(decodeFec));
        OpusException.ThrowOnError(length);
        return length;
    }

    public int Decode(ReadOnlySpan<byte> data, Span<float> pcm, bool decodeFec = false)
    {
        var frameSize = pcm.Length / ChannelCount;
        var length = _opus.DecodeFloat(
            _decoder,
            data[0],
            data.Length,
            out pcm[0],
            frameSize,
            Convert.ToInt32(decodeFec));
        OpusException.ThrowOnError(length);
        return length;
    }
}