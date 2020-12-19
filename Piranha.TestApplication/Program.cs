using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone;
using Piranha.Jawbone.OpenAl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Sqlite;

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
                .AddSdl2()
                .AddSingleton<IWindowManager, WindowManager>()
                .AddTransient<Random>()
                .AddSingleton<MyTestHandler>();
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
            var windowManager = serviceProvider.GetRequiredService<IWindowManager>();
            var handler = serviceProvider.GetRequiredService<MyTestHandler>();
            // var handler = serviceProvider.GetRequiredService<TestRenderHandler>();
            windowManager.AddWindow("Test Application", 1024, 768, handler);
            windowManager.Run();
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
