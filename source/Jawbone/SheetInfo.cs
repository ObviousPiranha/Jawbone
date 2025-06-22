using Jawbone.Sdl3;
using Jawbone.Stb;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Jawbone;

public sealed class SheetInfo : IDisposable
{
    public Point32 SheetSize { get; set; }
    public required ReadOnlyDictionary<string, SheetPosition> SheetPositionByFile { get; init; }
    public required Outbox<ImmutableArray<nint>> SdlSurfaces { get; init; }

    public static SheetInfo Create(Point32 sheetSize, string imageFolder)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(sheetSize.X);
        ArgumentOutOfRangeException.ThrowIfNegative(sheetSize.Y);
        ArgumentException.ThrowIfNullOrWhiteSpace(imageFolder);

        var sdlSurfaces = ImmutableArray.CreateBuilder<nint>();
        var sheetPositionByFile = new Dictionary<string, SheetPosition>();
        var sheetBuilder = new SheetBuilder(sheetSize);

        var imageSizes = new List<(string file, int pixelCount)>();
        {
            var pendingFolders = new Stack<string>();
            pendingFolders.Push(imageFolder);

            while (pendingFolders.TryPop(out var folder))
            {
                foreach (var innerFolder in Directory.EnumerateDirectories(folder))
                    pendingFolders.Push(innerFolder);
                
                foreach (var file in Directory.EnumerateFiles(folder, "*.png"))
                {
                    var imageSize = Png.Png.GetImageSize(file);
                    var pixelCount = imageSize.X * imageSize.Y;
                    imageSizes.Add((file, pixelCount));
                }
            }
        }

        imageSizes.Sort(static (a, b) => b.pixelCount.CompareTo(a.pixelCount));

        var srcRect = default(SdlRect);
        var dstRect = default(SdlRect);

        foreach (var image in imageSizes)
        {
            var bytes = File.ReadAllBytes(image.file);
            var imageData = StbImage.LoadFromMemory(
                bytes[0],
                bytes.Length,
                out var imageWidth,
                out var imageHeight,
                out var comp,
                4);
            var imageSurface = Sdl.CreateSurfaceFrom(
                imageWidth, imageHeight, SdlPixelFormat.Abgr8888, imageData, 4 * imageWidth);

            try
            {
                var sheetPosition = sheetBuilder.Allocate(new(imageWidth + 2, imageHeight + 2));
                if (sheetPosition == default)
                {
                    throw new InvalidOperationException(
                        $"No room for sprite '{image.file}'. Sheet is {sheetSize.X}x{sheetSize.Y}. Sprite is {imageWidth}x{imageHeight}.");
                }

                if (sdlSurfaces.Count == sheetPosition.SheetIndex)
                {
                    var blankSurface = Sdl.CreateSurface(
                        sheetSize.X,
                        sheetSize.Y,
                        SdlPixelFormat.Abgr8888);
                    blankSurface.ThrowOnSdlFailure("Unable to create surface.");
                    sdlSurfaces.Add(blankSurface);
                }
                else if (sdlSurfaces.Count < sheetPosition.SheetIndex)
                {
                    throw new InvalidOperationException("Skipped sheet creation.");
                }

                sheetPositionByFile.Add(
                    image.file,
                    new SheetPosition(sheetPosition.SheetIndex, sheetPosition.Rectangle.Padded(1)));

                var sheetSurface = sdlSurfaces[sheetPosition.SheetIndex];
                var block = sheetPosition.Rectangle;

                dstRect.X = block.Position.X + 1;
                dstRect.Y = block.Position.Y + 1;

                Sdl.BlitSurface(
                    imageSurface,
                    Unsafe.NullRef<SdlRect>(),
                    sheetSurface,
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");

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
                    dstRect).ThrowOnSdlFailure("Unable to blit surface.");
            }
            finally
            {
                Sdl.DestroySurface(imageSurface);
                StbImage.ImageFree(imageData);
            }
        }

        var result = new SheetInfo
        {
            SheetSize = sheetSize,
            SheetPositionByFile = new(sheetPositionByFile),
            SdlSurfaces = new(sdlSurfaces.DrainToImmutable())
        };

        return result;
    }

    public void Dispose()
    {
        if (SdlSurfaces.TryTake(out var sdlSurfaces) && !sdlSurfaces.IsDefaultOrEmpty)
        {
            foreach (var sdlSurface in sdlSurfaces)
                Sdl.DestroySurface(sdlSurface);
        }
    }
}