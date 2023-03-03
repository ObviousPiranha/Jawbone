using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Bcm;
using Piranha.Jawbone.Tools;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Jawbone.Sdl;

public static class SdlExtensions
{
    private const string BcmLibrary = "/opt/vc/lib/libbcm_host.so";

    public static IServiceCollection AddSdl2(this IServiceCollection services)
    {
        return services.AddSdl2(SdlInit.Everything);
    }

    public static IServiceCollection AddSdl2(
        this IServiceCollection services,
        uint flags)
    {
        var isVideoEnabled = (flags & SdlInit.Video) == SdlInit.Video;
        if (isVideoEnabled && RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && File.Exists(BcmLibrary))
        {
            services.AddNativeLibrary<IBcm>(
                _ => NativeLibraryInterface.FromFile<IBcm>(
                    BcmLibrary, name => name));
        }

        return services
            .AddSingleton<SdlLibrary>(
                serviceProvider =>
                {
                    var bcm = serviceProvider.GetService<IBcm>();
                    bcm?.HostInit();

                    var library = new SdlLibrary(flags);
                    return library;
                })
            .AddSingleton<ISdl2>(
                serviceProvider => serviceProvider.GetRequiredService<SdlLibrary>().Library);
    }

    public static IServiceCollection AddWindowManager(this IServiceCollection services)
    {
        return services.AddSingleton<IWindowManager, WindowManager>();
    }

    public static IServiceCollection AddAudioManager(this IServiceCollection services)
    {
        return services.AddSingleton<IAudioManager, AudioManager>();
    }

    public static void ThrowException(this ISdl2 sdl)
    {
        var message = sdl.GetError();
        throw new SdlException(message);
    }

    public static short[] ConvertAudioToInt16(
        this ISdl2 sdl,
        ReadOnlySpan<short> pcm,
        int sourceFrequency,
        int sourceChannels,
        int destinationFrequency,
        int destinationChannels)
    {
        var stream = sdl.NewAudioStream(
            (ushort)SdlAudio.S16Lsb,
            (byte)sourceChannels,
            sourceFrequency,
            (ushort)SdlAudio.S16Lsb,
            (byte)destinationChannels,
            destinationFrequency);

        if (stream.IsInvalid())
            sdl.ThrowException();

        try
        {
            // https://wiki.libsdl.org/SDL_AudioStreamPut
            var result = sdl.AudioStreamPut(stream, pcm[0], pcm.Length * Unsafe.SizeOf<short>());

            if (result != 0)
                sdl.ThrowException();

            // https://wiki.libsdl.org/SDL_AudioStreamFlush
            result = sdl.AudioStreamFlush(stream);

            if (result != 0)
                sdl.ThrowException();

            var length = sdl.AudioStreamAvailable(stream);

            if ((length & 1) != 0)
                throw new SdlException("Audio data must align to 2 bytes.");

            if (0 < length)
            {
                var shorts = new short[length / Unsafe.SizeOf<short>()];
                var bytesRead = sdl.AudioStreamGet(stream, out shorts[0], length);

                if (bytesRead == -1)
                    sdl.ThrowException();

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
        this ISdl2 sdl,
        ReadOnlySpan<short> pcm,
        int sourceFrequency,
        int sourceChannels,
        int destinationFrequency,
        int destinationChannels)
    {
        var stream = sdl.NewAudioStream(
            (ushort)SdlAudio.S16Lsb,
            (byte)sourceChannels,
            sourceFrequency,
            (ushort)SdlAudio.F32,
            (byte)destinationChannels,
            destinationFrequency);

        if (stream.IsInvalid())
            sdl.ThrowException();

        try
        {
            // https://wiki.libsdl.org/SDL_AudioStreamPut
            var result = sdl.AudioStreamPut(stream, pcm[0], pcm.Length * Unsafe.SizeOf<short>());

            if (result != 0)
                sdl.ThrowException();

            // https://wiki.libsdl.org/SDL_AudioStreamFlush
            result = sdl.AudioStreamFlush(stream);

            if (result != 0)
                sdl.ThrowException();

            var length = sdl.AudioStreamAvailable(stream);

            if ((length & 3) != 0)
                throw new SdlException("Audio data must align to 4 bytes.");

            if (0 < length)
            {
                var floats = new float[length / Unsafe.SizeOf<float>()];
                var bytesRead = sdl.AudioStreamGet(stream, out floats[0], length);

                if (bytesRead == -1)
                    sdl.ThrowException();

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
