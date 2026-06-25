using Jawbone;
using Jawbone.Sdl3;
using Jawbone.Stb;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

internal partial class Program
{
    private static void Main(string[] args)
    {
        try
        {
            foreach (var arg in args)
                PackFolder(arg);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    static void PackFolder(string folder)
    {
        var stopwatch = Stopwatch.GetTimestamp();
        var sheetSurface = SdlExtensions.CreateSpriteSheet(
            folder,
            out var sheetSize,
            out var imageLocations);
        Console.WriteLine($"Sheet created in {Stopwatch.GetElapsedTime(stopwatch)}");

        var pixels = SdlSurface.FromPointer(sheetSurface).Pixels;
        StbImageWrite.WritePng(
            "sheet.png",
            sheetSize.X,
            sheetSize.Y,
            4,
            pixels,
            sheetSize.X * 4);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new Rectangle32.SimpleJsonConverter() }
        };

        using var stream = File.Create("sheet.json");
        JsonSerializer.Serialize(stream, imageLocations, jsonSerializerOptions);
    }
}