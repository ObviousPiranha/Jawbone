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
    private readonly List<float[]> _sounds = [];
    private readonly List<nint> _streams = [];
    private readonly ILogger<AudioManager> _logger;
    private readonly uint _device;
    private readonly SdlAudioSpec _expectedAudioSpec;
    private readonly SdlAudioSpec _actualAudioSpec;

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
        foreach (var stream in _streams)
        {
            Sdl.UnbindAudioStream(stream);
            Sdl.DestroyAudioStream(stream);
        }
        Sdl.CloseAudioDevice(_device);
        _logger.LogInformation("Disposed audio manager");
    }

    public void PumpAudio()
    {
        if (IsPaused)
            return;
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
            Sdl.PutAudioStreamData(stream, in data[0], data.Length).ThrowOnSdlFailure("Unable to put stream data.");
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

    public int PlayAudio(int soundId)
    {
        var sound = _sounds[soundId];
        var stream = GetAvailableStream();
        var result = Sdl.PutAudioStreamData(
            stream,
            in sound[0],
            sound.Length * Unsafe.SizeOf<float>());

        result.ThrowOnSdlFailure("Unable to put stream data.");

        Sdl.FlushAudioStream(stream).ThrowOnSdlFailure("Unable to flush audio stream.");
        return 0;
    }

    private nint GetAvailableStream()
    {
        foreach (var stream in _streams)
        {
            if (Sdl.GetAudioStreamAvailable(stream) == 0)
                return stream;
        }

        _logger.LogInformation("Creating audio stream {n}", _streams.Count);
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

        _streams.Add(newStream);

        return newStream;
    }
}
