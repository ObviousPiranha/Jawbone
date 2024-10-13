using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

sealed class LoopingAudio : IDisposable
{
    private static readonly SdlAudioStreamCallback SdlCallback = Callback;

    private readonly GCHandle _handle;
    private readonly nint _stream;

    private float[] _audio = [];
    private int _offset;
    private bool _playing;

    public LoopingAudio(in SdlAudioSpec spec)
    {
        _handle = GCHandle.Alloc(this);
        _stream = Sdl.CreateAudioStream(spec, spec);
        Sdl.SetAudioStreamGetCallback(_stream, SdlCallback, (nint)_handle)
            .ThrowOnSdlFailure("Unable to set stream callback.");
        if (_stream == default)
            SdlException.Throw("Unable to create audio stream.");
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
        _playing = true;
        Sdl.BindAudioStream(deviceId, _stream)
            .ThrowOnSdlFailure("Unable to bind audio stream");
    }

    public void Stop()
    {
        if (!_playing)
            return;
        _playing = false;
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
        if (_audio.Length == 0)
            return;
        var remaining = amount;
        var offset = _offset;

        while (0 < remaining)
        {
            var available = _audio.Length - offset;

            if (available <= remaining)
            {
                Sdl.PutAudioStreamData(_stream, in _audio[offset], available * Unsafe.SizeOf<float>())
                    .ThrowOnSdlFailure("Unable to put audio stream data.");
                remaining -= available;
                offset = 0;
            }
            else
            {
                Sdl.PutAudioStreamData(_stream, in _audio[offset], remaining * Unsafe.SizeOf<float>())
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
        var obj = handle.Target as LoopingAudio;
        if (obj is null)
            return;
        Debug.Assert(stream == obj._stream);
        Debug.Assert(0 < totalAmount);
        Debug.Assert((totalAmount & 3) == 0);
        obj.Update(totalAmount / 4);
    }
}
