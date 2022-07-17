using System;
using System.IO;
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
            encoder.Channels);

        var opusSampleCount = encoder.SamplingRate / 50; // 20ms
        var bufferForEncoding = new byte[8192];
        var pcm = fullPcm.AsSpan(0, opusSampleCount * encoder.Channels);
        var encodedLength = encoder.Encode(pcm, bufferForEncoding);
        var encoded = bufferForEncoding.AsSpan(0, encodedLength);
        var debugView = bufferForEncoding.AsSpan(encodedLength - 4); // Just making sure it becomes all zero after the end.

        var encodedWord = encodedLength == 1 ? "byte" : "bytes";
        Console.WriteLine($"Encoded to {encodedLength} {encodedWord}.");

        using var decoder = new OpusDecoder(opus);
        var destinationPcm = new short[pcm.Length];
        var decodedLength = decoder.Decode(encoded, destinationPcm, true);
        var decodedWord = decodedLength == 1 ? "sample" : "samples";
        Console.WriteLine($"Decoded to {decodedLength} {decodedWord}.");
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