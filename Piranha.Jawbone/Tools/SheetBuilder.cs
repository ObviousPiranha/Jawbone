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
            Span<int> fitValues = stackalloc int[6]
            {
                int.MaxValue, // best fit (low)
                int.MaxValue, // best fit (high)
                int.MaxValue, // best sheet index
                0, // current low fit
                0, // current high fit
                0 // current sheet index
            };

            for (int i = 0; i < _available.Count; ++i)
            {
                var fit = _available[i].Rectangle.Size - size;
                fit.InOrder(out fitValues[3], out fitValues[4]);
                fitValues[5] = _available[i].SheetIndex;

                if (0 <= fitValues[3] && Comparer.IsLessThan(fitValues[3..6], fitValues[0..3]))
                {
                    bestIndex = i;
                    fitValues[3..6].CopyTo(fitValues);
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
