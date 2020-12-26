using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;

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

        static void CreateSheetsFromFolder(IServiceProvider serviceProvider, string folder)
        {
            
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
                    foreach (var arg in args)
                        CreateSheetsFromFolder(serviceProvider, arg);
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
