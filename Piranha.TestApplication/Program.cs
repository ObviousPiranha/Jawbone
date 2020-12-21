using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone;
using Piranha.Jawbone.OpenAl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Sqlite;
using Piranha.Jawbone.Stb;

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
                .AddSingleton<IWindowManager, WindowManager>();
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
                var windowManager = serviceProvider.GetRequiredService<IWindowManager>();
                var handler = ActivatorUtilities.CreateInstance<MyTestHandler>(serviceProvider, new Random());
                // var handler = serviceProvider.GetRequiredService<TestRenderHandler>();
                var w = 1024;
                var h = 768;
                if (fullscreen)
                {
                    var sdl = serviceProvider.GetRequiredService<ISdl2>();
                    var result = sdl.GetCurrentDisplayMode(0, out var mode);

                    if (result < 0)
                        throw new SdlException("Unable to get display mode: " + sdl.GetError());
                    
                    w = mode.w;
                    h = mode.h;
                }

                windowManager.AddWindow("Test Application", w, h, fullscreen, handler);
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
