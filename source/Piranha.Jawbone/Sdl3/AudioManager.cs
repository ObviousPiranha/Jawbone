using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Sdl3;

sealed class AudioManager : IAudioManager, IDisposable
{
    private readonly Dictionary<int, LoopingAudio> _loopingAudio = [];
    private readonly Stack<LoopingAudio> _loopingAudioPool = [];
    private readonly List<float[]> _sounds = [];
    private readonly Dictionary<int, nint> _streamsById = [];
    private readonly ILogger<AudioManager> _logger;
    private readonly uint _device;
    private readonly SdlAudioSpec _expectedAudioSpec;
    private readonly SdlAudioSpec _actualAudioSpec;

    private int _nextPlayId = 1;
    private int _nextLoopId = 1;

    public bool IsPaused
    {
        get
        {
            return Sdl.AudioDevicePaused(_device);
        }

        set
        {
            if (value)
                Sdl.PauseAudioDevice(_device);
            else
                Sdl.ResumeAudioDevice(_device);
        }
    }

    public float Gain
    {
        get
        {
            var result = Sdl.GetAudioDeviceGain(_device);
            if (result < 0f)
                SdlException.Throw("Unable to get audio gain.");
            return result;
        }

        set
        {
            Sdl.SetAudioDeviceGain(_device, value)
                .ThrowOnSdlFailure("Unable to set gain.");
        }
    }

    public AudioManager(
        ILogger<AudioManager> logger)
    {
        _logger = logger;

        _expectedAudioSpec = new SdlAudioSpec
        {
            Freq = 48000,
            Format = SdlAudioFormat.F32,
            Channels = 2
        };

        var deviceIdsPointer = Sdl.GetAudioPlaybackDevices(out var deviceCount);

        try
        {
            var deviceIds = deviceIdsPointer.ToReadOnlySpan<uint>(deviceCount);
            var deviceNames = "(none)";

            if (!deviceIds.IsEmpty)
            {
                var stringBuilder = new StringBuilder();
                var deviceName = Sdl.GetAudioDeviceName(deviceIds[0]).ToString() ?? "(none)";
                stringBuilder.Append(deviceName);

                for (int i = 1; i < deviceIds.Length; ++i)
                {
                    deviceName = Sdl.GetAudioDeviceName(deviceIds[i]).ToString() ?? "(none)";
                    stringBuilder.Append(", ").Append(deviceName);
                }

                deviceNames = stringBuilder.ToString();
            }

            _logger.LogInformation("Audio devices: {devices}", deviceNames);
        }
        finally
        {
            Sdl.Free(deviceIdsPointer);
        }

        _device = Sdl.OpenAudioDevice(
            SdlAudioDeviceDefault.Playback,
            in _expectedAudioSpec);
        _actualAudioSpec = _expectedAudioSpec;

        if (_device == 0)
            SdlException.Throw("Unable to open audio device.");
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing looping audio...");
        foreach (var loopingAudio in _loopingAudio.Values)
            loopingAudio.Dispose();
        _loopingAudio.Clear();
        while (_loopingAudioPool.TryPop(out var loopingAudio))
            loopingAudio.Dispose();
        _logger.LogInformation("Disposing other audio...");
        foreach (var stream in _streamsById.Values)
        {
            Sdl.UnbindAudioStream(stream);
            Sdl.DestroyAudioStream(stream);
        }
        _streamsById.Clear();
        _logger.LogInformation("Closing audio device...");
        Sdl.CloseAudioDevice(_device);
        _logger.LogInformation("Disposed audio manager!");
    }

    private int PrepareAudio(
        SdlAudioFormat format,
        int frequency,
        int channels,
        ReadOnlySpan<byte> data)
    {
        var sourceSpec = new SdlAudioSpec
        {
            Format = format,
            Channels = channels,
            Freq = frequency
        };
        var stream = Sdl.CreateAudioStream(
            in sourceSpec,
            in _expectedAudioSpec);

        if (stream.IsInvalid())
            SdlException.Throw();

        try
        {
            Sdl.PutAudioStreamData(stream, in data[0], data.Length)
                .ThrowOnSdlFailure("Unable to put stream data.");
            Sdl.FlushAudioStream(stream).ThrowOnSdlFailure("Unable to flush.");

            var length = Sdl.GetAudioStreamAvailable(stream);

            if ((length & 3) != 0)
                throw new SdlException("Audio data must align to 4 bytes.");

            if (0 < length)
            {
                var floats = new float[length / Unsafe.SizeOf<float>()];
                var bytesRead = Sdl.GetAudioStreamData(stream, out floats[0], length);

                if (bytesRead == -1)
                    SdlException.Throw();

                var soundIndex = _sounds.Count;
                _sounds.Add(floats);
                return soundIndex;
            }
            else
            {
                throw new SdlException("Empty audio data.");
            }
        }
        finally
        {
            Sdl.DestroyAudioStream(stream);
        }
    }

    public int PrepareAudio(
        int frequency,
        int channels,
        ReadOnlySpan<short> data)
    {
        return PrepareAudio(
            SdlAudioFormat.S16,
            frequency,
            channels,
            MemoryMarshal.AsBytes(data));
    }

    public int PrepareAudio(
        int frequency,
        int channels,
        ReadOnlySpan<float> data)
    {
        return PrepareAudio(
            SdlAudioFormat.F32,
            frequency,
            channels,
            MemoryMarshal.AsBytes(data));
    }

    public int PlayAudio(int soundId, float gain, float ratio)
    {
        var sound = _sounds[soundId];
        var pair = GetAvailableStream();
        Sdl.SetAudioStreamGain(pair.Value, gain)
            .ThrowOnSdlFailure("Unable to set stream gain.");
        Sdl.SetAudioStreamFrequencyRatio(pair.Value, ratio)
            .ThrowOnSdlFailure("Unable to set stream ratio.");
        var result = Sdl.PutAudioStreamData(
            pair.Value,
            in sound[0],
            sound.ByteSize());

        result.ThrowOnSdlFailure("Unable to put stream data.");

        Sdl.FlushAudioStream(pair.Value)
            .ThrowOnSdlFailure("Unable to flush audio stream.");
        return pair.Key;
    }

    public bool TrySetGain(int playbackId, float gain)
    {
        if (_streamsById.TryGetValue(playbackId, out var stream))
        {
            Sdl.SetAudioStreamGain(stream, gain)
                .ThrowOnSdlFailure("Unable to set stream gain.");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TrySetRatio(int playbackId, float ratio)
    {
        if (_streamsById.TryGetValue(playbackId, out var stream))
        {
            Sdl.SetAudioStreamFrequencyRatio(stream, ratio)
                .ThrowOnSdlFailure("Unable to set stream ratio.");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryStopAudio(int playbackId)
    {
        if (_streamsById.TryGetValue(playbackId, out var stream))
        {
            Sdl.ClearAudioStream(stream)
                .ThrowOnSdlFailure("Unable to clear audio stream.");
            return true;
        }
        else
        {
            return false;
        }
    }

    public int LoopAudio(
        int soundId,
        float gain,
        float ratio)
    {
        var audio = _sounds[soundId];
        if (!_loopingAudioPool.TryPop(out var loopingAudio))
            loopingAudio = new LoopingAudio(_actualAudioSpec);

        loopingAudio.Start(_device, audio, gain, ratio);
        _loopingAudio.Add(_nextLoopId, loopingAudio);
        return _nextLoopId++;
    }

    public bool TrySetLoopGain(int loopId, float gain)
    {
        if (_loopingAudio.TryGetValue(loopId, out var loopingAudio))
        {
            loopingAudio.SetGain(gain);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TrySetLoopRatio(int loopId, float ratio)
    {
        if (_loopingAudio.TryGetValue(loopId, out var loopingAudio))
        {
            loopingAudio.SetRatio(ratio);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryStopLoop(int loopId)
    {
        if (_loopingAudio.Remove(loopId, out var loopingAudio))
        {
            loopingAudio.Stop();
            _loopingAudioPool.Push(loopingAudio);
            return true;
        }
        else
        {
            return false;
        }
    }

    private KeyValuePair<int, nint> GetAvailableStream()
    {
        var found = default(KeyValuePair<int, nint>);
        foreach (var pair in _streamsById)
        {
            if (Sdl.GetAudioStreamAvailable(pair.Value) == 0)
            {
                found = pair;
                break;
            }
        }

        if (found.Key != 0)
        {
            _streamsById.Remove(found.Key);
            _streamsById.Add(_nextPlayId, found.Value);
            return KeyValuePair.Create(_nextPlayId++, found.Value);
        }
        else
        {
            _logger.LogInformation("Creating audio stream {n}", _streamsById.Count);
            var newStream = Sdl.CreateAudioStream(in _expectedAudioSpec, in _actualAudioSpec);

            if (newStream == default)
                SdlException.Throw("Failed to make new audio stream.");

            try
            {
                Sdl.BindAudioStream(_device, newStream).ThrowOnSdlFailure("Unable to bind audio stream.");
            }
            catch
            {
                Sdl.DestroyAudioStream(newStream);
                throw;
            }

            _streamsById.Add(_nextPlayId, newStream);
            return KeyValuePair.Create(_nextPlayId++, newStream);
        }
    }
}
