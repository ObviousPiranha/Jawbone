using Jawbone;
using Jawbone.Sdl3;
using Jawbone.Stb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        var imageSizes = new List<KeyValuePair<string, Point32>>();

        foreach (var file in Directory.EnumerateFiles(folder))
        {
            var imageSize = Jawbone.Png.Png.GetImageSize(file);
            var pair = KeyValuePair.Create(Path.GetFileName(file), imageSize);
            imageSizes.Add(pair);
        }

        imageSizes.Sort(ComparePairs);

        // foreach (var pair in imageSizes)
        // {
        //     Console.WriteLine(pair);
        // }

        Console.WriteLine();
        Console.WriteLine("--- --- ---");
        Console.WriteLine();

        var sheetSize = new Point32(1200);
        var sheetSurface = Sdl.CreateSurface(sheetSize.X, sheetSize.Y, SdlPixelFormat.Abgr8888)
            .ThrowOnSdlFailure("Failed to create surface.");
        var sheetBuilder = new SheetBuilder(sheetSize);
        var imageLocations = new Dictionary<string, Rectangle32>();

        foreach (var pair in imageSizes)
        {
            var sizeInAtlas = pair.Value + 2;
            var sheetPosition = sheetBuilder.Allocate(sizeInAtlas);
            if (0 < sheetPosition.SheetIndex)
            {
                Console.WriteLine("oops " + pair);
                continue;
            }
            var spritePosition = sheetPosition.Rectangle.Padded(1);
            imageLocations.Add(pair.Key, spritePosition);
            Console.WriteLine($"{pair.Key}: {spritePosition}");
            var file = Path.Combine(folder, pair.Key);
            var imageSurface = Sdl.LoadPng(file)
                .ThrowOnSdlFailure("Failed to load file.");
            {
                var block = sheetPosition.Rectangle;
                var imageWidth = spritePosition.Size.X;
                var imageHeight = spritePosition.Size.Y;
                var srcRect = default(SdlRect);
                var dstRect = default(SdlRect);

                dstRect.X = block.Position.X + 1;
                dstRect.Y = block.Position.Y + 1;

                Sdl.BlitSurface(
                    imageSurface,
                    Unsafe.NullRef<SdlRect>(),
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Top edge
                srcRect.X = 0;
                srcRect.Y = 0;
                srcRect.W = imageWidth;
                srcRect.H = 1;
                dstRect.X = block.Position.X + 1;
                dstRect.Y = block.Position.Y;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Bottom edge
                srcRect.X = 0;
                srcRect.Y = imageHeight - 1;
                srcRect.W = imageWidth;
                srcRect.H = 1;
                dstRect.X = block.Position.X + 1;
                dstRect.Y = block.Position.Y + 1 + imageHeight;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Left edge
                srcRect.X = 0;
                srcRect.Y = 0;
                srcRect.W = 1;
                srcRect.H = imageHeight;
                dstRect.X = block.Position.X;
                dstRect.Y = block.Position.Y + 1;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Right edge
                srcRect.X = imageWidth - 1;
                srcRect.Y = 0;
                srcRect.W = 1;
                srcRect.H = imageHeight;
                dstRect.X = block.Position.X + 1 + imageWidth;
                dstRect.Y = block.Position.Y + 1;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Top left corner
                srcRect.X = 0;
                srcRect.Y = 0;
                srcRect.W = 1;
                srcRect.H = 1;
                dstRect.X = block.Position.X;
                dstRect.Y = block.Position.Y;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Top right corner
                srcRect.X = imageWidth - 1;
                srcRect.Y = 0;
                srcRect.W = 1;
                srcRect.H = 1;
                dstRect.X = block.Position.X + 1 + imageWidth;
                dstRect.Y = block.Position.Y;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Bottom left corner
                srcRect.X = 0;
                srcRect.Y = imageHeight - 1;
                srcRect.W = 1;
                srcRect.H = 1;
                dstRect.X = block.Position.X;
                dstRect.Y = block.Position.Y + 1 + imageHeight;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();

                // Bottom right corner
                srcRect.X = imageWidth - 1;
                srcRect.Y = imageHeight - 1;
                srcRect.W = 1;
                srcRect.H = 1;
                dstRect.X = block.Position.X + 1 + imageWidth;
                dstRect.Y = block.Position.Y + 1 + imageHeight;
                Sdl.BlitSurface(
                    imageSurface,
                    srcRect,
                    sheetSurface,
                    dstRect
                    ).ThrowOnSdlFailure();
            }
            Sdl.DestroySurface(imageSurface);
        }

        var pixels = SdlSurface.FromPointer(sheetSurface).Pixels;
        StbImageWrite.WritePng(
            "sheet.png",
            sheetSize.X,
            sheetSize.Y,
            4,
            pixels,
            sheetSize.X * 4);

        Sdl.DestroySurface(sheetSurface);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new Rectangle32.SimpleJsonConverter() }
        };

        using var stream = File.Create("sheet.json");
        JsonSerializer.Serialize(stream, imageLocations, jsonSerializerOptions);
    }

    static int ComparePairs(
        KeyValuePair<string, Point32> a,
        KeyValuePair<string, Point32> b)
    {
        var area0 = a.Value.X * a.Value.Y;
        var area1 = b.Value.X * b.Value.Y;
        var result = area1.CompareTo(area0);
        if (result != 0)
            return result;
        var extreme0 = int.Max(a.Value.X, a.Value.Y);
        var extreme1 = int.Max(b.Value.X, b.Value.Y);
        result = extreme1.CompareTo(extreme0);
        if (result != 0)
            return result;
        
        result = a.Key.CompareTo(b.Key);
        return result;
    }
}