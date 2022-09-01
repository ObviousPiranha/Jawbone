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
    static ServiceProvider CreateServiceProvider()
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

        return services.BuildServiceProvider(options);
    }
    
    static void EncodeAndThenDecode()
    {
        using var serviceProvider = CreateServiceProvider();
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
        var opusDecoded = new short[pcm.Length * 16];
        var opusDecodedSampleCount = decoder.Decode(opusEncoded, opusDecoded);
        var decodedWord = opusDecodedSampleCount == 1 ? "sample" : "samples";
        logger.LogInformation("Decoded to {0} {1}.", opusDecodedSampleCount, decodedWord);
        stb.PiranhaFree(oggBuffer);
    }

    static short[] ReadAllShorts(string path)
    {
        var bytes = File.ReadAllBytes(path);
        if ((bytes.Length & 1) == 1)
            throw new Exception("Odd byte count in " + path);
        var result = new short[bytes.Length / 2];
        Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
        return result;
    }

    static void TestVector(string inputFile, string outputFile)
    {
        // https://chromium.googlesource.com/chromium/deps/opus/+/1.1.1/doc/trivial_example.c
        using var serviceProvider = CreateServiceProvider();
        var opusProvider = serviceProvider.GetRequiredService<OpusProvider>();

        var inputBytes = File.ReadAllBytes(inputFile);
        var outputBytes = File.ReadAllBytes(outputFile);
        var buffer = new short[outputBytes.Length];

        var decoder = opusProvider.CreateDecoder();

        var n = decoder.Decode(inputBytes, buffer);
    }
    
    static void Main(string[] args)
    {
        try
        {
            EncodeAndThenDecode();

            // if (1 < args.Length)
            //     TestVector(args[0], args[1]);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
