using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Opus;

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
            .AddOpus();
        

        var options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };
        
        using var serviceProvider = services.BuildServiceProvider(options);
        var opus = serviceProvider.GetRequiredService<IOpus>();
        using var encoder = new OpusEncoder(opus);
        var sampleCount = encoder.SamplingRate / 50;
        var audio = new float[sampleCount * encoder.Channels];
        audio.AsSpan().Fill(0.5f);
        var encoded = new byte[8192];
        var length = encoder.Encode(audio, encoded);
    }

    static void Main(string[] args)
    {
        EncodeAndThenDecode();
    }
}