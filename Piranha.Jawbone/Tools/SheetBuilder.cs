using System;
using System.Collections.Generic;

namespace Piranha.Jawbone.Tools
{
    public sealed class SheetBuilder
    {
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
            var bestIndex = NoIndex;
            var bestFitLow = int.MaxValue;
            var bestFitHigh = int.MaxValue;

            for (int i = 0; i < _available.Count; ++i)
            {
                var fit = _available[i].Rectangle.Size - size;
                fit.InOrder(out var fitLow, out var fitHigh);

                if (0 <= fitLow && (fitLow < bestFitLow || (fitLow == bestFitLow && fitHigh < bestFitHigh)))
                {
                    bestIndex = i;
                    bestFitLow = fitLow;
                    bestFitHigh = fitHigh;
                }
            }

            if (bestIndex == NoIndex)
            {
                if (SheetSize.X < size.X || SheetSize.Y < size.Y)
                    throw new Exception("Cannot allocate space for item bigger than sheet.");
                
                bestIndex = _available.Count;
                _available.Add(
                    new SheetPosition(
                        _nextSheetIndex++,
                        new Rectangle32(default, SheetSize)));
            }

            var slot = _available[bestIndex];
            slot.Rectangle.Pack(size, out var rx, out var ry);

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
}
