namespace Piranha.Jawbone.Sdl3;

// typedef Uint32 SDL_AudioDeviceID;

public static class SdlAudioDeviceDefault
{
    // #define SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK ((SDL_AudioDeviceID) 0xFFFFFFFFu)
    public const uint Playback = uint.MaxValue;

    // #define SDL_AUDIO_DEVICE_DEFAULT_RECORDING ((SDL_AudioDeviceID) 0xFFFFFFFEu)
    public const uint Recording = uint.MaxValue - 1;
}
