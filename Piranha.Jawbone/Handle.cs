using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone;

public readonly struct Handle : IEquatable<Handle>
{
    public readonly int Index { get; init; }
    public readonly int Generation { get; init; }

    public bool Equals(Handle other) => Index == other.Index && Generation == other.Generation;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Handle other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Index, Generation);
    public override string? ToString() => $"{Index}:{Generation}";
}
