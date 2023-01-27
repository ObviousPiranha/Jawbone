using System;
using System.Collections.Generic;

namespace Piranha.Jawbone.Tools;

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
    private readonly List<SheetPosition> _available = new();

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
}
