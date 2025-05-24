using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl3;

sealed class LoopingAudio : IDisposable
{
    private static readonly SdlAudioStreamCallback SdlCallback = Callback;

    private readonly GCHandle _handle;
    private readonly nint _stream;

    private float[] _audio = [];
    private int _offset;

    public bool Playing { get; private set; }

    public LoopingAudio(in SdlAudioSpec spec)
    {
        _handle = GCHandle.Alloc(this);
        _stream = Sdl.CreateAudioStream(spec, spec);
        Sdl.SetAudioStreamGetCallback(_stream, SdlCallback, (nint)_handle)
            .ThrowOnSdlFailure("Unable to set stream callback.");
        if (_stream == default)
            SdlException.Throw("Unable to create audio stream.");
    }

    public void SetGain(float gain)
    {
        Sdl.SetAudioStreamGain(_stream, gain)
            .ThrowOnSdlFailure("Unable to set gain.");
    }

    public void SetRatio(float ratio)
    {
        Sdl.SetAudioStreamFrequencyRatio(_stream, ratio)
            .ThrowOnSdlFailure("Unable to set ratio.");
    }

    public unsafe void Start(uint deviceId, float[] audio, float gain, float ratio)
    {
        if (audio is null || audio.Length == 0)
            return;

        Stop();
        _audio = audio;
        _offset = 0;

        Sdl.SetAudioStreamGain(_stream, gain)
            .ThrowOnSdlFailure("Unable to set stream gain.");
        Sdl.SetAudioStreamFrequencyRatio(_stream, ratio)
            .ThrowOnSdlFailure("Unable to set stream ratio.");
        Playing = true;
        Sdl.BindAudioStream(deviceId, _stream)
            .ThrowOnSdlFailure("Unable to bind audio stream");
    }

    public void Stop()
    {
        if (!Playing)
            return;
        Playing = false;
        Sdl.UnbindAudioStream(_stream);
    }

    public void Dispose()
    {
        Stop();
        Sdl.DestroyAudioStream(_stream);
        _handle.Free();
    }

    private void Update(int amount)
    {
        var audio = _audio.AsSpan();
        if (audio.IsEmpty)
            return;
        var remaining = amount;
        var offset = _offset;

        while (0 < remaining)
        {
            var available = audio.Length - offset;

            if (available <= remaining)
            {
                Sdl.PutAudioStreamData(_stream, in audio[offset], available * Unsafe.SizeOf<float>())
                    .ThrowOnSdlFailure("Unable to put audio stream data.");
                remaining -= available;
                offset = 0;
            }
            else
            {
                Sdl.PutAudioStreamData(_stream, in audio[offset], remaining * Unsafe.SizeOf<float>())
                    .ThrowOnSdlFailure("Unable to put audio stream data.");
                offset += remaining;
                break;
            }
        }

        _offset = offset;
    }

    private static void Callback(
        nint userdata,
        nint stream,
        int additionalAmount,
        int totalAmount)
    {
        var handle = (GCHandle)userdata;
        if (handle.Target is not LoopingAudio obj)
            return;
        Debug.Assert(stream == obj._stream);
        Debug.Assert(0 < totalAmount);
        Debug.Assert((totalAmount & 3) == 0);
        obj.Update(totalAmount / 4);
    }
}
