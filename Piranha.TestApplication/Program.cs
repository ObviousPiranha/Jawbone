using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone;
using Piranha.Jawbone.OpenAl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Sqlite;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;

namespace Piranha.TestApplication
{
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
                .AddSqlite3()
                .AddSdl2()
                .AddStb()
                .AddWindowManager();
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

            try
            {
                var scenePool = new ScenePool<PiranhaScene>();
                var gameLoop = ActivatorUtilities.CreateInstance<GameLoop>(serviceProvider, scenePool);
                var windowManager = serviceProvider.GetRequiredService<IWindowManager>();
                var handler = ActivatorUtilities.CreateInstance<MyTestHandler>(serviceProvider, scenePool);
                var windowId = windowManager.AddWindow("Test Application", 1024, 768, fullscreen, handler);

                using (ActivatorUtilities.CreateInstance<GameLoopManager>(serviceProvider, gameLoop, handler))
                    windowManager.Run();
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
}
