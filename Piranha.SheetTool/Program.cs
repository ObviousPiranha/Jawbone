﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;
using Piranha.Jawbone.Tools.CollectionExtensions;

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

        static void CreateSheetsFromFolder(ILogger logger, ISdl2 sdl, IStb stb, string inputFolder, string outputFolder)
        {
            var surfaces = new List<IntPtr>();
            var nameBySurface = new Dictionary<IntPtr, string>();
            var folders = new Stack<string>();
            folders.Push(inputFolder);

            while (folders.TryPop(out var folder))
            {
                logger.LogDebug("Processing folder {0}", folder);

                foreach (var innerFolder in Directory.EnumerateDirectories(folder))
                    folders.Push(innerFolder);
                
                foreach (var png in Directory.EnumerateFiles(folder, "*.png"))
                {
                    logger.LogDebug("Loading {0}", png);
                    var pngBytes = File.ReadAllBytes(png);
                    var pixelBytes = stb.StbiLoadFromMemory(
                        pngBytes[0],
                        pngBytes.Length,
                        out var width,
                        out var height,
                        out var comp,
                        4);
                    
                    if (pixelBytes.IsInvalid())
                        throw new Exception("Unable to load PNG.");
                    
                    var surface = sdl.CreateRGBSurfaceFrom(
                        pixelBytes,
                        width,
                        height,
                        32,
                        4 * width,
                        Platform.Rmask,
                        Platform.Gmask,
                        Platform.Bmask,
                        Platform.Amask);
                    
                    if (surface.IsInvalid())
                    {
                        stb.StbiImageFree(pixelBytes);
                        throw new SdlException("Unable to create surface: " + sdl.GetError());
                    }
                    
                    surfaces.Add(surface);
                    nameBySurface.Add(surface, Path.GetFileNameWithoutExtension(png));
                }

            }

            surfaces.Sort(
                (a, b) =>
                {
                    var sa = new SurfaceView(a);
                    var sb = new SurfaceView(b);
                    var areaA = sa.Width * sa.Height;
                    var areaB = sb.Width * sb.Height;
                    return -areaA.CompareTo(areaB); // Sort descending!
                });

            var infoPath = Path.Combine(outputFolder, "info.json");
            var options = new JsonWriterOptions
            {
                Indented = true
            };
            using (var builder = new SheetImageBuilder(stb, sdl, new Point32(1024, 1024)))
            using (var fileStream = File.Create(infoPath))
            using (var writer = new Utf8JsonWriter(fileStream, options))
            {
                writer.WriteStartObject();
                foreach (var surface in surfaces)
                {
                    var position = builder.Add(surface);
                    writer.WritePropertyName(nameBySurface[surface]);
                    writer.WriteStartObject();
                    writer.WriteNumber("sheetIndex", position.SheetIndex);
                    writer.WriteNumber("x", position.Rectangle.Position.X);
                    writer.WriteNumber("y", position.Rectangle.Position.Y);
                    writer.WriteNumber("width", position.Rectangle.Size.X);
                    writer.WriteNumber("height", position.Rectangle.Size.Y);
                    writer.WriteEndObject();
                    writer.Flush();
                }
                writer.WriteEndObject();
                
                builder.SaveImages(outputFolder);
            }

            foreach (var surface in surfaces)
            {
                var view = new SurfaceView(surface);
                stb.StbiImageFree(view.Pixels);
                sdl.FreeSurface(surface);
            }
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

                    CreateSheetsFromFolder(logger, sdl, stb, args[0], args[1]);
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
