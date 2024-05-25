using System;

namespace Piranha.Jawbone;

public readonly struct SheetPosition : IEquatable<SheetPosition>
{
    public readonly Rectangle32 Rectangle;
    public readonly int SheetIndex;

    public SheetPosition(int sheetIndex, Rectangle32 rectangle)
    {
        SheetIndex = sheetIndex;
        Rectangle = rectangle;
    }

    public readonly bool Equals(SheetPosition other) => SheetIndex == other.SheetIndex && Rectangle == other.Rectangle;
    public override readonly bool Equals(object? obj) => obj is SheetPosition other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(SheetIndex, Rectangle);
    public override readonly string ToString() => $"index {SheetIndex} {Rectangle}";

    public static bool operator ==(SheetPosition left, SheetPosition right) => left.Equals(right);
    public static bool operator !=(SheetPosition left, SheetPosition right) => !left.Equals(right);
}
