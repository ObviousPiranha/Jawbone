using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Jawbone.OpenAl
{
    class AudioSystem : IAudioSystem, IDisposable
    {
        private readonly IOpenAl _al;
        private readonly IStb _stb;
        private readonly ILogger<AudioSystem> _logger;
        private readonly Dictionary<string, int> _bufferIndexBySoundId = new Dictionary<string, int>();
        private readonly List<uint> _buffers = new List<uint>();
        private readonly uint[] _sources = new uint[8];
        private IntPtr _device;
        private IntPtr _context;
        private int _nextSource = 0;
        private int _musicSource = 0;
        private ushort[] _idGenerations;

        public AudioSystem(
            IOpenAl al,
            IStb stb,
            ILogger<AudioSystem> logger)
        {
            _idGenerations = new ushort[_sources.Length];

            _al = al;
            _stb = stb;
            _logger = logger;

            _device = _al.OpenDevice(null);

            if (_device != IntPtr.Zero)
            {
                _context = _al.CreateContext(_device, null);

                if (_context != IntPtr.Zero)
                {
                    _al.MakeContextCurrent(_context);
                    _logger.LogDebug("OpenAL started.");

                    _al.GenSources(_sources.Length, out _sources[0]);
                }
                else
                {
                    _logger.LogError("error on alcCreateContext");
                }
            }
            else
            {
                _logger.LogError("error on alcOpenDevice");
            }
        }

        public void Dispose()
        {
            if (_device.IsValid())
            {
                _al.CloseDevice(_device);
                _logger.LogDebug("closed OpenAL device");
            }
            
            if (_context.IsValid())
            {
                _al.DeleteSources(_sources.Length, _sources[0]);

                _al.MakeContextCurrent(IntPtr.Zero);
                _al.DestroyContext(_context);
            }
        }

        public int GetBufferIndex(string soundId) => _bufferIndexBySoundId[soundId];

        public void AddAudioSource(string soundId, ReadOnlySpan<byte> data)
        {
            var wavHeader = new WavHeader(data);

            if (wavHeader.DataOffset != 0)
            {
                _al.GenBuffers(1, out var buffer);
                _al.BufferData(
                    buffer,
                    wavHeader.Format == 1 ? Al.FormatMono16 : Al.FormatStereo16,
                    data[wavHeader.DataOffset],
                    wavHeader.ChunkSize,
                    wavHeader.SampleRate * 2); // The great mystery...

                _bufferIndexBySoundId.Add(soundId, _buffers.Count);
                _buffers.Add(buffer);
            }
            else
            {
                // Not a WAV file? Must be OGG!

                int samples = _stb.StbVorbisDecodeMemory(
                    data[0],
                    data.Length,
                    out var channelCount,
                    out var sampleRate,
                    out var output);
                
                if (output.IsValid())
                {
                    try
                    {
                        _al.GenBuffers(1, out var buffer);
                        _al.BufferData(
                            buffer,
                            channelCount == 1 ? Al.FormatMono16 : Al.FormatStereo16,
                            output,
                            samples * 4,
                            sampleRate);
                        
                        _bufferIndexBySoundId.Add(soundId, _buffers.Count);
                        _buffers.Add(buffer);
                    }
                    finally
                    {
                        _stb.PiranhaFree(output);
                    }
                }
                else
                {
                    throw new OpenAlException("Unable to load audio as WAV or OGG.");
                }
            }
        }

        public int Play(int soundId, bool looping, float gain = 1.0f)
        {
            _logger.LogTrace("Attempting to play sound " + soundId);

            var buffer = _buffers[soundId];
            int playingSource = -1;

            for (int i = 0; playingSource == -1 && i < _sources.Length; i++)
            {
                int currentSourceIndex = (_nextSource + i) % _sources.Length;
                var currentSource = _sources[currentSourceIndex];
                _al.GetSourcei(currentSource, Al.SourceState, out var sourceState);
                if (sourceState == Al.Initial || sourceState == Al.Stopped)
                {
                    _al.Sourcei(currentSource, Al.Buffer, buffer);
                    _al.Sourcef(currentSource, Al.MaxGain, 1.0f);
                    _al.Sourcei(currentSource, Al.Looping, Convert.ToInt32(looping));
                    _al.Sourcef(currentSource, Al.Gain, gain);
                    _al.SourcePlay(currentSource);
                    _nextSource = (currentSourceIndex + 1) % _sources.Length;
                    playingSource = currentSourceIndex;
                    _logger.LogTrace("Using source " + currentSourceIndex);
                }
                else
                {
                    _logger.LogTrace("Source " + currentSourceIndex + " already playing.");
                }
            }

            return FuseIdGen((ushort)playingSource,unchecked(++_idGenerations[playingSource]));
        }
        public bool Stop(int soundId)
        {
            var idGenPair = GetIdGenPair(soundId);

            if (SourceGenIsValid(idGenPair))
            {
                _logger.LogTrace("Stopping source " + idGenPair.id);
                _al.SourceStop(_sources[idGenPair.id]);
            }
            else
            {
                _logger.LogTrace("Source " + idGenPair.id + " expired.");
            }

            return true; // TODO: Actually do something.
        }

        public void PlayMusic(int soundId)
        {
            _musicSource = Play(soundId, true, .5f);
        }

        public void StopMusic()
        {
            Stop(_musicSource);
        }

        public void SetMasterVolume(float volume)
        {
            _logger.LogDebug("Setting master volume " + volume);
            _al.Listenerf(Al.Gain, volume.Clamped(0f, 1f));
        }

        private bool SourceGenIsValid((short id, short gen) idGenPair)
        {
            return _idGenerations[idGenPair.id] == idGenPair.gen;
        }

        private (short id, short gen) GetIdGenPair(int idgen)
        {
            var id = (short)(idgen & 0xffff);
            var gen = (short)(idgen >> 16);
            return (id,gen);
        }

        private static int FuseIdGen(ushort id, ushort gen)
        {
            return (gen << 16) | id;
        }
    };  
}
