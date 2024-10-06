using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Sdl3;

public static class SdlExtensions
{
    public static IServiceCollection AddAudioManager(this IServiceCollection services)
    {
        return services.AddSingleton<IAudioManager, AudioManager>();
    }

    public static short[] ConvertAudioToInt16(
        ReadOnlySpan<short> pcm,
        int sourceFrequency,
        int sourceChannels,
        int destinationFrequency,
        int destinationChannels)
    {
        var srcSpec = new SdlAudioSpec
        {
            Format = SdlAudioFormat.S16,
            Channels = sourceChannels,
            Freq = sourceFrequency
        };
        var dstSpec = new SdlAudioSpec
        {
            Format = SdlAudioFormat.S16,
            Channels = destinationChannels,
            Freq = destinationFrequency
        };
        var stream = Sdl.CreateAudioStream(in srcSpec, in dstSpec);

        if (stream.IsInvalid())
            SdlException.Throw();

        try
        {
            var result = Sdl.PutAudioStreamData(
                stream,
                in pcm[0],
                pcm.Length * Unsafe.SizeOf<short>());

            if (result != 0)
                SdlException.Throw();

            result = Sdl.FlushAudioStream(stream);

            if (result != 0)
                SdlException.Throw();

            var length = Sdl.GetAudioStreamAvailable(stream);

            if ((length & 1) != 0)
                throw new SdlException("Audio data must align to 2 bytes.");

            if (0 < length)
            {
                var shorts = new short[length / Unsafe.SizeOf<short>()];
                var bytesRead = Sdl.GetAudioStreamData(stream, out shorts[0], length);

                if (bytesRead == -1)
                    SdlException.Throw();

                return shorts;
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

    public static float[] ConvertAudioToFloat32(
        ReadOnlySpan<short> pcm,
        int sourceFrequency,
        int sourceChannels,
        int destinationFrequency,
        int destinationChannels)
    {
        var srcSpec = new SdlAudioSpec
        {
            Format = SdlAudioFormat.S16,
            Channels = sourceChannels,
            Freq = sourceFrequency
        };
        var dstSpec = new SdlAudioSpec
        {
            Format = SdlAudioFormat.F32,
            Channels = destinationChannels,
            Freq = destinationFrequency
        };
        var stream = Sdl.CreateAudioStream(in srcSpec, in dstSpec);

        if (stream.IsInvalid())
            SdlException.Throw();

        try
        {
            var result = Sdl.PutAudioStreamData(stream, in pcm[0], pcm.Length * Unsafe.SizeOf<short>());

            if (result != 0)
                SdlException.Throw();

            result = Sdl.FlushAudioStream(stream);

            if (result != 0)
                SdlException.Throw();

            var length = Sdl.GetAudioStreamAvailable(stream);

            if ((length & 3) != 0)
                throw new SdlException("Audio data must align to 4 bytes.");

            if (0 < length)
            {
                var floats = new float[length / Unsafe.SizeOf<float>()];
                var bytesRead = Sdl.GetAudioStreamData(stream, out floats[0], length);

                if (bytesRead == -1)
                    SdlException.Throw();

                return floats;
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
}
