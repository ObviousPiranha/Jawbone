using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Jawbone;

public sealed class SheetBuilder
{
    private static void InnerPack(
        Rectangle32 r,
        Point32 size,
        int rightHeight,
        int bottomWidth,
        out Rectangle32 rx,
        out Rectangle32 ry)
    {
        rx = new Rectangle32(
            new Point32(r.Position.X + size.X, r.Position.Y),
            new Point32(r.Size.X - size.X, rightHeight));
        ry = new Rectangle32(
            new Point32(r.Position.X, r.Position.Y + size.Y),
            new Point32(bottomWidth, r.Size.Y - size.Y));
    }

    private static void SmartPack(
        Rectangle32 space,
        Point32 size,
        out Rectangle32 rx,
        out Rectangle32 ry)
    {
        var fit = space.Size - size;

        if (fit.X < fit.Y)
        {
            InnerPack(
                space,
                size,
                size.Y,
                space.Size.X,
                out rx,
                out ry);
        }
        else
        {
            InnerPack(
                space,
                size,
                space.Size.Y,
                size.X,
                out rx,
                out ry);
        }
    }
    private const int NoIndex = -1;
    private readonly List<SheetPosition> _available = [];

    public Point32 SheetSize { get; }

    private int _nextSheetIndex = 0;

    public SheetBuilder(Point32 sheetSize)
    {
        SheetSize = sheetSize;
    }

    public SheetPosition Allocate(Point32 size)
    {
        if (!size.AllPositive() || SheetSize.X < size.X || SheetSize.Y < size.Y)
            return default;

        var bestIndex = NoIndex;
        var bestFit = SheetFit.WorstFit;

        for (int i = 0; i < _available.Count; ++i)
        {
            var gaps = _available[i].Rectangle.Size - size;
            var fit = SheetFit.Create(gaps);

            if (fit.IsValid && fit < bestFit)
            {
                bestFit = fit;
                bestIndex = i;
            }
        }

        if (bestIndex == NoIndex)
        {
            bestIndex = _available.Count;
            _available.Add(
                new SheetPosition(
                    _nextSheetIndex++,
                    new Rectangle32(default, SheetSize)));
        }

        var slot = _available[bestIndex];
        SmartPack(slot.Rectangle, size, out var rx, out var ry);

        if (rx.Size.AllPositive())
        {
            _available[bestIndex] = new SheetPosition(slot.SheetIndex, rx);

            if (ry.Size.AllPositive())
                _available.Add(new SheetPosition(slot.SheetIndex, ry));
        }
        else if (ry.Size.AllPositive())
        {
            _available[bestIndex] = new SheetPosition(slot.SheetIndex, ry);
        }
        else
        {
            _available.RemoveAt(bestIndex);
        }

        return new SheetPosition(
            slot.SheetIndex,
            new Rectangle32(
                slot.Rectangle.Position,
                size));
    }

    public static void CreateSingleSheet(
        string folder,
        out Point32 sheetSize,
        out Dictionary<string, Rectangle32> imageLocations)
    {
        var imageSizes = GetImageSizes(folder, out var totalArea, out var largestDimensions);
        imageSizes.Sort(ComparePairs);

        var minSheetEdge = int.Max(
            (int)float.Sqrt(totalArea),
            int.Max(largestDimensions.X, largestDimensions.Y));
        var sheetEdge = minSheetEdge * 2;
        imageLocations = new(imageSizes.Count);
        var imageLocationsCandidate = new Dictionary<string, Rectangle32>(imageSizes.Count);

        while (1 < sheetEdge - minSheetEdge)
        {
            var nextSheetEdge = (minSheetEdge + sheetEdge) / 2;
            var sheetBuilder = new SheetBuilder(new(nextSheetEdge));
            imageLocationsCandidate.Clear();

            foreach (var pair in imageSizes)
            {
                var sizeInAtlas = pair.Value + 2;
                var sheetPosition = sheetBuilder.Allocate(sizeInAtlas);
                if (0 < sheetPosition.SheetIndex)
                {
                    minSheetEdge = nextSheetEdge;
                    break;
                }
                var spritePosition = sheetPosition.Rectangle.Padded(1);
                imageLocationsCandidate.Add(pair.Key, spritePosition);
            }

            if (minSheetEdge != nextSheetEdge)
            {
                sheetEdge = nextSheetEdge;
                (imageLocations, imageLocationsCandidate) = (imageLocationsCandidate, imageLocations);
            }
        }

        sheetSize = new(sheetEdge);
        Debug.Assert(imageSizes.Count == imageLocations.Count);
    }

    public static void CreateSheets(
        string folder,
        Point32 sheetSize,
        out Dictionary<string, SheetPosition> imageLocations)
    {
        var imageSizes = GetImageSizes(folder, out var totalArea, out var largestDimensions);
        if (sheetSize.X < largestDimensions.X || sheetSize.Y < largestDimensions.Y)
            throw new InvalidOperationException($"Sheet size {sheetSize} is not big enough for largest sprites {largestDimensions}.");
        imageSizes.Sort(ComparePairs);

        var sheetBuilder = new SheetBuilder(sheetSize);
        imageLocations = new(imageSizes.Count);

        foreach (var pair in imageSizes)
        {
            var sizeInAtlas = pair.Value + 2;
            var sheetPosition = sheetBuilder.Allocate(sizeInAtlas);
            imageLocations.Add(pair.Key, sheetPosition.Padded(1));
        }

        Debug.Assert(imageSizes.Count == imageLocations.Count);
    }

    private static List<KeyValuePair<string, Point32>> GetImageSizes(
        string folder,
        out int totalAtlasArea,
        out Point32 largestDimensions)
    {
        var imageSizes = new List<KeyValuePair<string, Point32>>();
        totalAtlasArea = 0;
        largestDimensions = default;

        var pendingFolders = new Stack<string?>();
        pendingFolders.Push(null);
        while (pendingFolders.TryPop(out var relativeFolder))
        {
            var currentFolder = relativeFolder is null ? folder : Path.Combine(folder, relativeFolder);

            foreach (var innerFolder in Directory.EnumerateDirectories(currentFolder))
            {
                var pendingFolder = Path.GetFileName(innerFolder);
                if (relativeFolder is not null)
                    pendingFolder = Path.Combine(relativeFolder, pendingFolder);
                pendingFolders.Push(pendingFolder);
            }

            foreach (var file in Directory.EnumerateFiles(currentFolder, "*.png"))
            {
                var imageSize = Png.Png.GetImageSize(file);
                var relativeFile = Path.GetFileName(file);
                if (relativeFolder is not null)
                    relativeFile = Path.Combine(relativeFolder, relativeFile);
                relativeFile = relativeFile.Replace('\\', '/');
                var pair = KeyValuePair.Create(relativeFile, imageSize);
                imageSizes.Add(pair);
                var sizeInAtlas = imageSize + 1;
                totalAtlasArea += sizeInAtlas.X * sizeInAtlas.Y;
                largestDimensions = new(
                    int.Max(largestDimensions.X, sizeInAtlas.X),
                    int.Max(largestDimensions.Y, sizeInAtlas.Y));
            }
        }
        return imageSizes;
    }

    private static int ComparePairs(
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
