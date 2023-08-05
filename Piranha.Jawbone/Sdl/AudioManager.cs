using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

public sealed class AudioManager : IAudioManager, IDisposable
{
    private const int Frequency = 48000;
    private const SdlAudio Format = SdlAudio.F32;
    private const int Channels = 2;
    private const int SamplesPerSecond = Frequency * Channels;

    private readonly List<AudioShader> _shaders = new();
    private readonly List<float[]> _sounds = new();
    private readonly List<ScheduledAudio> _scheduledAudio = new();
    private readonly object _lock = new();
    private readonly GCHandle _handle;
    private readonly ISdl2 _sdl;
    private readonly ILogger<AudioManager> _logger;
    private readonly uint _device;

    private long _sampleIndex = 0;
    private int _nextId = 1;

    public AudioManager(
        ISdl2 sdl,
        ILogger<AudioManager> logger)
    {
        _sdl = sdl;
        _logger = logger;
        _handle = GCHandle.Alloc(this);

        try
        {
            IntPtr callback;

            unsafe
            {
                delegate*<IntPtr, IntPtr, int, void> fp = &Callback;
                callback = new IntPtr(fp);
            }

            var audioSpec = new AudioSpec
            {
                Freq = Frequency,
                Format = (ushort)Format,
                Channels = Channels,
                Samples = 4096,
                Callback = callback,
                Userdata = (IntPtr)_handle
            };

            _device = _sdl.OpenAudioDevice(null, 0, audioSpec, IntPtr.Zero, 0);

            if (_device == 0)
                _sdl.ThrowException();
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

        lock (_lock)
            _scheduledAudio.Clear();

        if (_handle.IsAllocated)
        {
            _sdl.CloseAudioDevice(_device);
            _handle.Free();
        }
    }

    public int PrepareAudio(
        SdlAudio format,
        int frequency,
        int channels,
        ReadOnlySpan<byte> data)
    {
        var stream = _sdl.NewAudioStream(
            (ushort)format,
            (byte)channels,
            frequency,
            (ushort)Format,
            Channels,
            Frequency);

        if (stream.IsInvalid())
            _sdl.ThrowException();

        try
        {
            // https://wiki.libsdl.org/SDL_AudioStreamPut
            var result = _sdl.AudioStreamPut(stream, data[0], data.Length);

            if (result != 0)
                _sdl.ThrowException();

            // https://wiki.libsdl.org/SDL_AudioStreamFlush
            result = _sdl.AudioStreamFlush(stream);

            if (result != 0)
                _sdl.ThrowException();

            var length = _sdl.AudioStreamAvailable(stream);

            if ((length & 3) != 0)
                throw new SdlException("Audio data must align to 4 bytes.");

            if (0 < length)
            {
                var floats = new float[length / Unsafe.SizeOf<float>()];
                var bytesRead = _sdl.AudioStreamGet(stream, out floats[0], length);

                if (bytesRead == -1)
                    _sdl.ThrowException();

                lock (_lock)
                {
                    var soundIndex = _sounds.Count;
                    _sounds.Add(floats);
                    return soundIndex;
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
        var delayOffset = (long)(delay.TotalSeconds * SamplesPerSecond);
        var gapDelay = delayBetweenLoops < TimeSpan.Zero ? -1 : (int)(delayBetweenLoops.TotalSeconds * SamplesPerSecond);

        lock (_lock)
        {
            var scheduledAudio = new ScheduledAudio
            {
                StartSampleIndex = _sampleIndex + delayOffset,
                Samples = _sounds[soundId],
                LoopDelaySampleCount = gapDelay,
                Id = unchecked(_nextId++)
            };

            _scheduledAudio.Add(scheduledAudio);

            if (_scheduledAudio.Count == 1)
                _sdl.PauseAudioDevice(_device, 0);

            return scheduledAudio.Id;
        }
    }

    public bool CancelAudio(int scheduledAudioId)
    {
        lock (_lock)
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

        return false;
    }

    private void AcquireData(Span<float> samples)
    {
        samples.Clear();
        lock (_lock)
        {
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
                        var lo = Math.Max(_sampleIndex, scheduledAudio.StartSampleIndex);
                        var hi = Math.Min(endSampleIndex, endAudioIndex);

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
                            var lo = Math.Max(_sampleIndex, scheduledAudio.StartSampleIndex);
                            var hi = Math.Min(endSampleIndex, endAudioIndex);

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
                    shader.Invoke(Frequency, Channels, samples);
            }

            // if (_scheduledAudio.Count == 0)
            //     _sdl.PauseAudioDevice(_device, 1);

            _sampleIndex = endSampleIndex;
        }
    }

    private unsafe static void Callback(IntPtr userdata, IntPtr data, int size)
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
        lock (_lock)
            _shaders.Add(audioShader);
    }

    public void RemoveShader(AudioShader audioShader)
    {
        lock (_lock)
            _shaders.Remove(audioShader);
    }
}
