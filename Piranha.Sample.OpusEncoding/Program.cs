using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Opus;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Sample.OpusEncoding;

static class Program
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
        var opus = serviceProvider.GetRequiredService<IOpus>();
        using var encoder = new OpusEncoder(opus);
        Console.WriteLine("Encoder bitrate: " + encoder.Bitrate);
        var audioBytes = File.ReadAllBytes("../Piranha.SampleApplication/crunch.ogg");
        int samples = stb.StbVorbisDecodeMemory(
            audioBytes[0],
            audioBytes.Length,
            out var channelCount,
            out var sampleRate,
            out var output);
        var rawPcm = output.ToReadOnlySpan<short>(samples * channelCount);
        var fullPcm = sdl.ConvertAudioToFloat(
            rawPcm,
            sampleRate,
            channelCount,
            encoder.SamplingRate,
            encoder.Channels);

        var sampleCount = encoder.SamplingRate / 50; // 20ms
        var encoded = new byte[8192];
        var pcm = fullPcm.AsSpan(0, sampleCount * encoder.Channels);
        var encodedLength = encoder.Encode(pcm, encoded);

        var encodedWord = encodedLength == 1 ? "byte" : "bytes";
        Console.WriteLine($"Encoded to {encodedLength} {encodedWord}.");

        using var decoder = new OpusDecoder(opus);
        var destinationPcm = new float[pcm.Length];
        var decodedLength = decoder.Decode(encoded.AsSpan(0, encodedLength), destinationPcm);
        var decodedWord = decodedLength == 1 ? "sample" : "samples";
        Console.WriteLine($"Decoded to {decodedLength} {decodedWord}.");
        stb.PiranhaFree(output);
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