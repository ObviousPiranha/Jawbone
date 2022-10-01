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
