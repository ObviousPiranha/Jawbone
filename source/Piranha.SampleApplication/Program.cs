using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone;
using Piranha.Jawbone.Sdl2;
using System;

namespace Piranha.SampleApplication;

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
            .AddSdl2()
            .AddJawboneNativeLibraries()
            .AddAudioManager()
            .AddSingleton<ScenePool<PiranhaScene>>()
            .AddSingleton<IGameLoop, GameLoop>()
            .AddSingleton<GameLoopManager>()
            .AddSingleton<SampleHandler>();
    }

    static void RunApplication(bool fullscreen)
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

        try
        {
            var handler = serviceProvider.GetRequiredService<SampleHandler>();
            //windowManager.AddWindow("Sample Application", 1024, 768, fullscreen, handler);
            var sdl = serviceProvider.GetRequiredService<Sdl2Library>();

            using (serviceProvider.GetRequiredService<GameLoopManager>())
                ApplicationManager.Run(sdl, handler);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error running window manager.");
        }
    }

    static void Main(string[] args)
    {
        try
        {
            RunApplication(0 < args.Length && args[0] == "fs");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ResetColor();
        }
    }
}
