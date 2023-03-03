using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Tools;

public static class ImmutableArrayFactory
{
    public static ImmutableArray<T> Create<T>(ReadOnlySpan<T> span)
    {
        var array = span.ToArray();

        // https://stackoverflow.com/a/51301665
        return Unsafe.As<T[], ImmutableArray<T>>(ref array);
    }
}

public static class ImmutableArrayFactory<T>
{
    public static ImmutableArray<T> Create<TState>(
        int length,
        TState state,
        SpanAction<T, TState> spanAction)
    {
        var array = new T[length];
        spanAction.Invoke(array, state);
        return Unsafe.As<T[], ImmutableArray<T>>(ref array);
    }
}
