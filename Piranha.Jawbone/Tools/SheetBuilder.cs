using System.Collections.Generic;

namespace Piranha.Jawbone.Tools
{
    public sealed class SheetBuilder
    {
        private const int NoIndex = -1;
        private readonly List<Rectangle32> _available = new();

        public Point32 SheetSize { get; }

        public SheetBuilder(Point32 sheetSize)
        {
            SheetSize = sheetSize;

            _available.Add(new Rectangle32(default, sheetSize));
        }


        public Rectangle32 Allocate(Point32 size)
        {
            var bestIndex = NoIndex;
            var bestFitLow = 0;
            var bestFitHigh = 0;

            for (int i = 0; i < _available.Count; ++i)
            {
                var fit = _available[i].Size - size;
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
                return default;
            }
            else
            {
                var slot = _available[bestIndex];
                var result = new Rectangle32(slot.Position, size);
                slot.Pack(size, out var rx, out var ry);

                if (rx.Size.AllPositive())
                {
                    _available[bestIndex] = rx;

                    if (ry.Size.AllPositive())
                        _available.Add(ry);
                }
                else if (ry.Size.AllPositive())
                {
                    _available[bestIndex] = ry;
                }
                else
                {
                    _available.RemoveAt(bestIndex);
                }

                return result;
            }
        }
    }
}
