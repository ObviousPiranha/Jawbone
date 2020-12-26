using System;

namespace Piranha.Jawbone.Tools
{
    public readonly struct SheetPosition : IEquatable<SheetPosition>
    {
        public readonly int SheetIndex;
        public readonly Rectangle32 Rectangle;

        public SheetPosition(int sheetIndex, Rectangle32 rectangle)
        {
            SheetIndex = sheetIndex;
            Rectangle = rectangle;
        }

        public bool Equals(SheetPosition other) => SheetIndex == other.SheetIndex && Rectangle == other.Rectangle;
        public override bool Equals(object? obj) => obj is SheetPosition other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(SheetIndex, Rectangle);
        public override string? ToString() => $"index {SheetIndex} {Rectangle}";

        public static bool operator ==(SheetPosition left, SheetPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SheetPosition left, SheetPosition right)
        {
            return !(left == right);
        }
    }
}
