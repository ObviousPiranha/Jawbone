using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Piranha.Jawbone.Tools;

public delegate void SpanAction<T>(Span<T> span);

public static class Few
{
    public static ImmutableArray<T> CreateImmutableArray<T>(int length, SpanAction<T> action)
    {
        var items = new T[length];
        action.Invoke(items);
        return Unsafe.As<T[], ImmutableArray<T>>(ref items);
    }

    public static Few<T> Create<T>(params T[]? items)
        => new Few<T>(ImmutableArray.Create<T>(items));
    public static Few<T> Create<T>(int count, SpanAction<T> action)
        => new Few<T>(CreateImmutableArray(count, action));
    public static Few<T> CreateRange<T>(IEnumerable<T> items)
        => new Few<T>(ImmutableArray.CreateRange(items));
}

public readonly struct Few<T> : IEquatable<Few<T>>
{
    private readonly ImmutableArray<T> _items;

    public readonly ImmutableArray<T> Items => _items.IsDefault ? ImmutableArray<T>.Empty : _items;

    public Few(ImmutableArray<T> items) => _items = items;
    public bool Equals(Few<T> other) => Items.SequenceEqual(other.Items);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Few<T> other && Equals(other);

    public override int GetHashCode()
    {
        if (_items.IsDefaultOrEmpty)
            return 0;

        var hashCode = new HashCode();

        foreach (var item in _items)
            hashCode.Add(item);

        var result = hashCode.ToHashCode();
        return result;
    }

    public override string? ToString()
    {
        if (_items.IsDefaultOrEmpty)
            return "[]";

        var builder = new StringBuilder("[").Append(Items[0]?.ToString());

        for (int i = 1; i < Items.Length; ++i)
            builder.Append(", ").Append(Items[i]?.ToString());

        var result = builder.Append(']').ToString();
        return result;
    }

    public static ImmutableArray<T> CreateImmutableArray<TState>(
        int length, TState state, SpanAction<T, TState> action)
    {
        var items = new T[length];
        action.Invoke(items, state);
        return Unsafe.As<T[], ImmutableArray<T>>(ref items);
    }

    public static Few<T> Create<TState>(
        int length, TState state, SpanAction<T, TState> action)
    {
        return new Few<T>(CreateImmutableArray(length, state, action));
    }

    public static bool operator ==(Few<T> a, Few<T> b) => a.Equals(b);
    public static bool operator !=(Few<T> a, Few<T> b) => !a.Equals(b);
}
