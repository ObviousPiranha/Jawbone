using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

sealed class AudioManager : IAudioManager, IDisposable
{
    private readonly List<AudioShader> _shaders = [];
    private readonly List<float[]> _sounds = [];
    private readonly List<ScheduledAudio> _scheduledAudio = [];
    private readonly GCHandle _handle;
    private readonly Sdl2Library _sdl;
    private readonly ILogger<AudioManager> _logger;
    private readonly uint _device;
    private readonly SdlAudioSpec _expectedAudioSpec;
    private readonly SdlAudioSpec _actualAudioSpec;

    private long _sampleIndex = 0;
    private int _nextId = 1;

    public bool IsPaused
    {
        get
        {
            var status = _sdl.GetAudioDeviceStatus(_device);
            return status == SdlAudioStatus.Paused;
        }

        set
        {
            var pauseOn = Convert.ToInt32(value);
            _sdl.PauseAudioDevice(_device, pauseOn);
        }
    }

    public AudioManager(
        Sdl2Library sdl,
        ILogger<AudioManager> logger)
    {
        _sdl = sdl;
        _logger = logger;
        _handle = GCHandle.Alloc(this);

        try
        {
            nint callback;

            unsafe
            {
                delegate*<nint, nint, int, void> fp = &Callback;
                callback = new nint(fp);
            }

            _expectedAudioSpec = new SdlAudioSpec
            {
                Freq = 48000,
                Format = SdlAudioFormat.F32,
                Channels = 2,
                Samples = 4096,
                Callback = callback,
                Userdata = (nint)_handle
            };

            var deviceCount = _sdl.GetNumAudioDevices(0);
            var devices = Enumerable
                .Range(0, deviceCount)
                .Select(n => _sdl.GetAudioDeviceName(n, 0).ToString() ?? "(null)");
            _logger.LogInformation("Audio devices: {devices}", string.Join(", ", devices));

            _device = _sdl.OpenAudioDevice(
                null,
                0,
                _expectedAudioSpec,
                out _actualAudioSpec,
                SdlAudioAllowChange.Any & ~SdlAudioAllowChange.Format);

            if (_device == 0)
                SdlException.Throw(sdl);
        }
        catch
        {
            _handle.Free();
            throw;
        }
    }

    public void Dispose()
    {
        // No point in making a finalizer.
        // This object holds onto a GC handle to itself.
        // It would never die on its own.

        // _logger.LogWarning("Ruh roh...");
        // IsPaused = true;
        // _sdl.PauseAudioDevice(_device, 1);

        _sdl.LockAudioDevice(_device);
        try
        {
            _scheduledAudio.Clear();
        }
        finally
        {
            _sdl.UnlockAudioDevice(_device);
        }

        var linuxDebug = OperatingSystem.IsLinux() && Debugger.IsAttached;

        if (linuxDebug)
            _logger.LogWarning("Attempting to pause SDL audio...");
        
        IsPaused = true;

        if (linuxDebug)
            _logger.LogWarning("Attempting to stop SDL audio...");
        
        if (_handle.IsAllocated)
        {
            // In Linux, this call hangs if a debugger is attached.
            _sdl.CloseAudioDevice(_device);
            _handle.Free();
        }

        _logger.LogInformation("Disposed audio manager");
    }

    public int PrepareAudio(
        SdlAudioFormat format,
        int frequency,
        int channels,
        ReadOnlySpan<byte> data)
    {
        var stream = _sdl.NewAudioStream(
            format,
            checked((byte)channels),
            frequency,
            _actualAudioSpec.Format,
            _actualAudioSpec.Channels,
            _actualAudioSpec.Freq);

        if (stream.IsInvalid())
            SdlException.Throw(_sdl);

        try
        {
            // https://wiki.libsdl.org/SDL_AudioStreamPut
            var result = _sdl.AudioStreamPut(stream, data[0], data.Length);

            if (result != 0)
                SdlException.Throw(_sdl);

            // https://wiki.libsdl.org/SDL_AudioStreamFlush
            result = _sdl.AudioStreamFlush(stream);

            if (result != 0)
                SdlException.Throw(_sdl);

            var length = _sdl.AudioStreamAvailable(stream);

            if ((length & 3) != 0)
                throw new SdlException("Audio data must align to 4 bytes.");

            if (0 < length)
            {
                var floats = new float[length / Unsafe.SizeOf<float>()];
                var bytesRead = _sdl.AudioStreamGet(stream, out floats[0], length);

                if (bytesRead == -1)
                    SdlException.Throw(_sdl);

                _sdl.LockAudioDevice(_device);
                try
                {
                    var soundIndex = _sounds.Count;
                    _sounds.Add(floats);
                    return soundIndex;
                }
                finally
                {
                    _sdl.UnlockAudioDevice(_device);
                }
            }
            else
            {
                throw new SdlException("Empty audio data.");
            }
        }
        finally
        {
            _sdl.FreeAudioStream(stream);
        }
    }

    public int ScheduleAudio(
        int soundId,
        TimeSpan delay)
    {
        return ScheduleLoopingAudio(soundId, delay, TimeSpan.MinValue);
    }

    public int ScheduleLoopingAudio(
        int soundId,
        TimeSpan delay,
        TimeSpan delayBetweenLoops)
    {
        var delayOffset = (long)(delay.TotalSeconds * _actualAudioSpec.Freq) * _actualAudioSpec.Channels;
        var gapDelay = delayBetweenLoops < TimeSpan.Zero ? -1 : (int)(delayBetweenLoops.TotalSeconds * _actualAudioSpec.Freq) * _actualAudioSpec.Channels;

        _sdl.LockAudioDevice(_device);
        try
        {
            var scheduledAudio = new ScheduledAudio
            {
                StartSampleIndex = _sampleIndex + delayOffset,
                Samples = _sounds[soundId],
                LoopDelaySampleCount = gapDelay,
                Id = unchecked(_nextId++)
            };

            _scheduledAudio.Add(scheduledAudio);

            if (_scheduledAudio.Count == 1 && IsPaused)
                IsPaused = false;
            
            return scheduledAudio.Id;
        }
        finally
        {
            _sdl.UnlockAudioDevice(_device);
        }
    }

    public bool CancelAudio(int scheduledAudioId)
    {
        _sdl.LockAudioDevice(_device);
        try
        {
            // Presumably, there will always be a relatively small number of items.
            // Even at the extreme end, there will be maybe 20 schedules.
            // As a result, linear search is fine.
            for (int i = 0; i < _scheduledAudio.Count; ++i)
            {
                if (_scheduledAudio[i].Id == scheduledAudioId)
                {
                    _scheduledAudio.RemoveAt(i);
                    return true;
                }
            }
        }
        finally
        {
            _sdl.UnlockAudioDevice(_device);
        }

        return false;
    }

    private void AcquireData(Span<float> samples)
    {
        samples.Clear();
        var endSampleIndex = _sampleIndex + samples.Length;
        var scheduledAudioIndex = 0;

        while (scheduledAudioIndex < _scheduledAudio.Count)
        {
            var scheduledAudio = _scheduledAudio[scheduledAudioIndex];

            if (0 <= scheduledAudio.LoopDelaySampleCount)
            {
                var fullSampleCount = scheduledAudio.Samples.Length + scheduledAudio.LoopDelaySampleCount;
                while (scheduledAudio.StartSampleIndex < endSampleIndex)
                {
                    var endAudioIndex = scheduledAudio.StartSampleIndex + scheduledAudio.Samples.Length;
                    var lo = long.Max(_sampleIndex, scheduledAudio.StartSampleIndex);
                    var hi = long.Min(endSampleIndex, endAudioIndex);

                    for (var i = lo; i < hi; ++i)
                    {
                        var index = (int)(i - _sampleIndex);
                        samples[index] += scheduledAudio.Samples[i - scheduledAudio.StartSampleIndex];
                    }

                    if (hi == endAudioIndex)
                    {
                        scheduledAudio = scheduledAudio with
                        {
                            StartSampleIndex =
                                scheduledAudio.StartSampleIndex +
                                scheduledAudio.Samples.Length +
                                scheduledAudio.LoopDelaySampleCount
                        };
                    }
                    else
                    {
                        break;
                    }
                }

                _scheduledAudio[scheduledAudioIndex++] = scheduledAudio;
            }
            else
            {
                var endAudioIndex = scheduledAudio.StartSampleIndex + scheduledAudio.Samples.Length;

                if (_sampleIndex < endAudioIndex)
                {
                    if (scheduledAudio.StartSampleIndex < endSampleIndex)
                    {
                        var lo = long.Max(_sampleIndex, scheduledAudio.StartSampleIndex);
                        var hi = long.Min(endSampleIndex, endAudioIndex);

                        for (var i = lo; i < hi; ++i)
                        {
                            var index = (int)(i - _sampleIndex);
                            samples[index] += scheduledAudio.Samples[i - scheduledAudio.StartSampleIndex];
                        }
                    }

                    ++scheduledAudioIndex;
                }
                else
                {
                    _scheduledAudio.RemoveAt(scheduledAudioIndex);
                }
            }

            foreach (var shader in _shaders)
                shader.Invoke(_actualAudioSpec.Freq, _actualAudioSpec.Channels, samples);
        }

        _sampleIndex = endSampleIndex;
    }

    private unsafe static void Callback(nint userdata, nint data, int size)
    {
        var handle = (GCHandle)userdata;
        var audioManager = handle.Target as AudioManager;

        if (audioManager is not null)
        {
            var span = new Span<float>(
                data.ToPointer(),
                size / Unsafe.SizeOf<float>());
            audioManager.AcquireData(span);
        }
    }

    public void AddShader(AudioShader audioShader)
    {
        _sdl.LockAudioDevice(_device);
        try
        {
            _shaders.Add(audioShader);
        }
        finally
        {
            _sdl.UnlockAudioDevice(_device);
        }
    }

    public void RemoveShader(AudioShader audioShader)
    {
        _sdl.LockAudioDevice(_device);
        try
        {
            _shaders.Remove(audioShader);
        }
        finally
        {
            _sdl.UnlockAudioDevice(_device);
        }
    }
}
