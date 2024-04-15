using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Sdl3;

public static class SdlExtensions
{
    public static IServiceCollection AddSdl3(
        this IServiceCollection services,
        SdlInit flags)
    {
        return services
            .AddSingleton(
                serviceProvider =>
                {
                    var path = Sdl3Provider.GetSdlPath();
                    var provider = new Sdl3Provider(path, flags);
                    return provider;
                })
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<Sdl3Provider>().Library);
    }

    public static IServiceCollection AddAudioManager(this IServiceCollection services)
    {
        return services.AddSingleton<IAudioManager, AudioManager>();
    }

    public static short[] ConvertAudioToInt16(
        this Sdl3Library sdl,
        ReadOnlySpan<short> pcm,
        int sourceFrequency,
        int sourceChannels,
        int destinationFrequency,
        int destinationChannels)
    {
        var stream = sdl.NewAudioStream(
            SdlAudioFormat.S16Lsb,
            (byte)sourceChannels,
            sourceFrequency,
            SdlAudioFormat.S16Lsb,
            (byte)destinationChannels,
            destinationFrequency);

        if (stream.IsInvalid())
            SdlException.Throw(sdl);

        try
        {
            // https://wiki.libsdl.org/SDL_AudioStreamPut
            var result = sdl.AudioStreamPut(stream, pcm[0], pcm.Length * Unsafe.SizeOf<short>());

            if (result != 0)
                SdlException.Throw(sdl);

            // https://wiki.libsdl.org/SDL_AudioStreamFlush
            result = sdl.AudioStreamFlush(stream);

            if (result != 0)
                SdlException.Throw(sdl);

            var length = sdl.AudioStreamAvailable(stream);

            if ((length & 1) != 0)
                throw new SdlException("Audio data must align to 2 bytes.");

            if (0 < length)
            {
                var shorts = new short[length / Unsafe.SizeOf<short>()];
                var bytesRead = sdl.AudioStreamGet(stream, out shorts[0], length);

                if (bytesRead == -1)
                    SdlException.Throw(sdl);

                return shorts;
            }
            else
            {
                throw new SdlException("Empty audio data.");
            }
        }
        finally
        {
            sdl.FreeAudioStream(stream);
        }
    }

    public static float[] ConvertAudioToFloat32(
        this Sdl3Library sdl,
        ReadOnlySpan<short> pcm,
        int sourceFrequency,
        int sourceChannels,
        int destinationFrequency,
        int destinationChannels)
    {
        var stream = sdl.NewAudioStream(
            SdlAudioFormat.S16Lsb,
            (byte)sourceChannels,
            sourceFrequency,
            SdlAudioFormat.F32,
            (byte)destinationChannels,
            destinationFrequency);

        if (stream.IsInvalid())
            SdlException.Throw(sdl);

        try
        {
            // https://wiki.libsdl.org/SDL_AudioStreamPut
            var result = sdl.AudioStreamPut(stream, pcm[0], pcm.Length * Unsafe.SizeOf<short>());

            if (result != 0)
                SdlException.Throw(sdl);

            // https://wiki.libsdl.org/SDL_AudioStreamFlush
            result = sdl.AudioStreamFlush(stream);

            if (result != 0)
                SdlException.Throw(sdl);

            var length = sdl.AudioStreamAvailable(stream);

            if ((length & 3) != 0)
                throw new SdlException("Audio data must align to 4 bytes.");

            if (0 < length)
            {
                var floats = new float[length / Unsafe.SizeOf<float>()];
                var bytesRead = sdl.AudioStreamGet(stream, out floats[0], length);

                if (bytesRead == -1)
                    SdlException.Throw(sdl);

                return floats;
            }
            else
            {
                throw new SdlException("Empty audio data.");
            }
        }
        finally
        {
            sdl.FreeAudioStream(stream);
        }
    }
}
