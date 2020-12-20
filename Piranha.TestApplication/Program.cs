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

        static void RunApplication()
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
                windowManager.AddWindow("Test Application", 1024, 768, handler);
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
                RunApplication();
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
