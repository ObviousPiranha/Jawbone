using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone;

readonly struct SheetFit : IEquatable<SheetFit>, IComparable<SheetFit>
{
    public static SheetFit Create(Point32 point) => Create(point.X, point.Y);
    public static SheetFit Create(int fit1, int fit2)
        => fit2 < fit1 ? new(fit2, fit1) : new(fit1, fit2);

    public static readonly SheetFit WorstFit = new(int.MaxValue, int.MaxValue);
    public static readonly SheetFit BestFit = new();
    public static readonly SheetFit InvalidFit = new(-1, -2);

    public readonly int PrimaryFit;
    public readonly int SecondaryFit;

    public readonly bool IsValid => 0 <= PrimaryFit && PrimaryFit <= SecondaryFit;

    private SheetFit(int primaryFit, int secondaryFit)
    {
        PrimaryFit = primaryFit;
        SecondaryFit = secondaryFit;
    }

    public bool Equals(SheetFit other)
        => PrimaryFit == other.PrimaryFit && SecondaryFit == other.SecondaryFit;
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is SheetFit other && Equals(other);
    public override int GetHashCode()
        => HashCode.Combine(PrimaryFit, SecondaryFit);
    public override string? ToString()
        => $"{PrimaryFit}, {SecondaryFit}";

    public int CompareTo(SheetFit other)
    {
        var result = PrimaryFit.CompareTo(other.PrimaryFit);

        if (result == 0)
            result = SecondaryFit.CompareTo(other.SecondaryFit);

        return result;
    }

    public static bool operator ==(SheetFit a, SheetFit b) => a.Equals(b);
    public static bool operator !=(SheetFit a, SheetFit b) => !a.Equals(b);
    public static bool operator <(SheetFit a, SheetFit b) => a.CompareTo(b) < 0;
    public static bool operator >(SheetFit a, SheetFit b) => a.CompareTo(b) > 0;
    public static bool operator <=(SheetFit a, SheetFit b) => a.CompareTo(b) <= 0;
    public static bool operator >=(SheetFit a, SheetFit b) => a.CompareTo(b) >= 0;
}
