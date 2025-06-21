using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Jawbone;
using Jawbone.Sdl3;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Piranha.SampleApplication3;

class Program
{
    static void ConfigureServices(IServiceCollection services)
    {
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
            .AddAudioManager()
            .AddSingleton<ScenePool<PiranhaScene>>()
            .AddSingleton<IGameLoop, GameLoop>()
            .AddSingleton<GameLoopManager>()
            .AddSingleton<SampleHandler>();
    }

    static void RunApplication()
    {
        Sdl.Init(SdlInit.Video | SdlInit.Audio | SdlInit.Events | SdlInit.Camera).ThrowOnSdlFailure("Unable to initialize SDL.");
        Sdl.SetAppMetadata("Jawbone SDL3 Sample", "1.0").ThrowOnSdlFailure("Unable to set app metadata.");

        try
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var options = new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            };

            using var serviceProvider = serviceCollection.BuildServiceProvider(options);
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            using (var process = System.Diagnostics.Process.GetCurrentProcess())
            {
                logger.LogInformation("Process ID - {pid}", process.Id);
            }

            var handler = serviceProvider.GetRequiredService<SampleHandler>();
            //windowManager.AddWindow("Sample Application", 1024, 768, fullscreen, handler);

            using (serviceProvider.GetRequiredService<GameLoopManager>())
                ApplicationManager.Run(handler);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
        finally
        {
            Sdl.Quit();
        }
    }

    static void Main(string[] args)
    {
        try
        {
            var ev = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry pair in ev)
                Console.WriteLine($"{pair.Key}={pair.Value}");
            RunApplication();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
