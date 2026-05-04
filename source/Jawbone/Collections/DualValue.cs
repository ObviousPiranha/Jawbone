using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Jawbone;

public readonly struct DualValue<TLeft, TRight> : IEquatable<DualValue<TLeft, TRight>>
{
    public TLeft Left { get; init; }
    public TRight Right { get; init; }

    public DualValue(TLeft left, TRight right)
    {
        Left = left;
        Right = right;
    }

    public bool Equals(DualValue<TLeft, TRight> other)
    {
        return
            EqualityComparer<TLeft>.Default.Equals(Left, other.Left) &&
            EqualityComparer<TRight>.Default.Equals(Right, other.Right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is DualValue<TLeft, TRight> other && Equals(other);
    }

    public override int GetHashCode() => HashCode.Combine(Left, Right);
    public override string ToString() => $"[{Left}, {Right}]";

    public static bool operator ==(DualValue<TLeft, TRight> a, DualValue<TLeft, TRight> b) => a.Equals(b);
    public static bool operator !=(DualValue<TLeft, TRight> a, DualValue<TLeft, TRight> b) => !a.Equals(b);
}

public static class DualValue
{
    public static DualValue<TLeft, TRight> Create<TLeft, TRight>(TLeft left, TRight right) => new(left, right);

    internal static bool TryGetSpan<T>(
        IEnumerable<T> enumerable,
        out ReadOnlySpan<T> span)
    {
        if (enumerable is T[] array)
        {
            span = array;
            return true;
        }

        if (enumerable is List<T> list)
        {
            span = CollectionsMarshal.AsSpan(list);
            return true;
        }

        span = default;
        return false;
    }
}