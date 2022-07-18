using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Opus;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Sample.OpusEncoding;

class Program
{
    static void EncodeAndThenDecode()
    {
        var services = new ServiceCollection();
        services
            .AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder
                    .AddSimpleConsole(
                        options =>
                        {
                            options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
                            options.IncludeScopes = true;
                        });
            })
            .AddSdl2()
            .AddStb()
            .AddOpus();
        

        var options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };
        
        using var serviceProvider = services.BuildServiceProvider(options);
        var sdl = serviceProvider.GetRequiredService<ISdl2>();
        var stb = serviceProvider.GetRequiredService<IStb>();
        var opusProvider = serviceProvider.GetRequiredService<OpusProvider>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        using var encoder = opusProvider.CreateEncoder();
        logger.LogInformation("Encoder bitrate: {0}", encoder.Bitrate);
        var oggBytes = File.ReadAllBytes("../Piranha.SampleApplication/crunch.ogg");
        int oggSampleCount = stb.StbVorbisDecodeMemory(
            oggBytes[0],
            oggBytes.Length,
            out var oggChannelCount,
            out var oggSampleRate,
            out var oggBuffer);
        var oggRawPcm = oggBuffer.ToReadOnlySpan<short>(oggSampleCount * oggChannelCount);
        var fullPcm = sdl.ConvertAudioToInt16(
            oggRawPcm,
            oggSampleRate,
            oggChannelCount,
            encoder.SamplingRate,
            encoder.ChannelCount);

        var opusSampleCount = encoder.SamplingRate / 50; // 20ms
        var opusBuffer = new byte[8192];
        var pcm = fullPcm.AsSpan(0, opusSampleCount * encoder.ChannelCount);
        var opusLength = encoder.Encode(pcm, opusBuffer);
        var opusEncoded = opusBuffer.AsSpan(0, opusLength);
        var debugView = opusBuffer.AsSpan(opusLength - 4); // Just making sure it becomes all zero after the end.

        var encodedWord = opusLength == 1 ? "byte" : "bytes";
        logger.LogInformation("Encoded to {0} {1}.", opusLength, encodedWord);

        using var decoder = opusProvider.CreateDecoder();
        var opusDecoded = new short[pcm.Length];
        var opusDecodedSampleCount = decoder.Decode(opusEncoded, opusDecoded, true);
        var decodedWord = opusDecodedSampleCount == 1 ? "sample" : "samples";
        logger.LogInformation("Decoded to {0} {1}.", opusDecodedSampleCount, decodedWord);
        stb.PiranhaFree(oggBuffer);
    }

    static void Main(string[] args)
    {
        try
        {
            EncodeAndThenDecode();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
