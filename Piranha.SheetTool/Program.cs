using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;

namespace Piranha.SheetTool
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
                .AddStb();
        }

        static void CreateSheetsFromFolder(ISdl2 sdl, IStb stb, string inputFolder, string outputFolder)
        {
            var builder = new SheetImageBuilder(stb, sdl, new Point32(1024, 1024));
        }
        
        static void Main(string[] args)
        {
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

                try
                {
                    var sdl = serviceProvider.GetRequiredService<ISdl2>();
                    var stb = serviceProvider.GetRequiredService<IStb>();

                    CreateSheetsFromFolder(sdl, stb, args[0], args[1]);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error building sheet.");
                }
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
