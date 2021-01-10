using System;

namespace Piranha.Jawbone.OpenAl
{
    public interface IAudioSystem
    {
        void AddAudioSource(string soundId, ReadOnlySpan<byte> data);
        int Play(int soundId, bool looping, float gain = 1f);
        void SetMasterVolume(float volume);
        int GetBufferIndex(string soundId);
    }
}
