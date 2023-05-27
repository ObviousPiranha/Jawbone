using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.SheetTool;

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

    static void CreateSheetsFromFolder(
        ILogger logger,
        ISdl2 sdl,
        IStb stb,
        Point32 sheetSize,
        string inputFolder,
        string outputFolder)
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

            foreach (var pngFile in Directory.EnumerateFiles(folder, "*.png"))
            {
                logger.LogDebug("Loading {0}", pngFile);
                var pngBytes = File.ReadAllBytes(pngFile);
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
                nameBySurface.Add(surface, Path.GetFileNameWithoutExtension(pngFile));
            }
        }

        surfaces.Sort(
            (a, b) =>
            {
                var viewA = new SurfaceView(a);
                var viewB = new SurfaceView(b);
                var areaA = viewA.Width * viewA.Height;
                var areaB = viewB.Width * viewB.Height;
                return -areaA.CompareTo(areaB); // Sort descending!
            });

        var infoPath = Path.Combine(outputFolder, "info.json");
        var options = new JsonWriterOptions
        {
            Indented = true,
            SkipValidation = true
        };

        using (var builder = new SheetImageBuilder(stb, sdl, sheetSize))
        using (var fileStream = File.Create(infoPath))
        using (var writer = new Utf8JsonWriter(fileStream, options))
        {
            writer.WriteStartObject();
            writer.WriteNumber("width", sheetSize.X);
            writer.WriteNumber("height", sheetSize.Y);
            writer.WritePropertyName("sprites");
            writer.WriteStartArray();
            foreach (var surface in surfaces)
            {
                var position = builder.Add(surface);
                writer.WriteStartObject();
                writer.WriteString("name", nameBySurface[surface]);
                writer.WriteNumber("sheetIndex", position.SheetIndex);
                writer.WriteNumber("x", position.Rectangle.Position.X);
                writer.WriteNumber("y", position.Rectangle.Position.Y);
                writer.WriteNumber("width", position.Rectangle.Size.X);
                writer.WriteNumber("height", position.Rectangle.Size.Y);
                writer.WriteEndObject();
                writer.Flush();
            }
            writer.WriteEndArray();
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

                var w = int.Parse(args[0]);
                var h = int.Parse(args[1]);

                CreateSheetsFromFolder(logger, sdl, stb, new Point32(w, h), args[2], args[3]);
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
