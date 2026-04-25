using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Extensions;

public static class CollectionExtensions
{
    public static SpanWithIndexEnumerable<T> WithIndex<T>(this T[]? array)
    {
        return new(array);
    }

    public static SpanWithIndexEnumerable<T> WithIndex<T>(this List<T>? list)
    {
        return new(CollectionsMarshal.AsSpan(list));
    }

    public static SpanWithIndexEnumerable<T> WithIndex<T>(this ReadOnlySpan<T> span)
    {
        return new(span);
    }

    public static IndexEnumerable<T> EnumerateIndicesOf<T>(this ReadOnlySpan<T> span, T needle)
    {
        return new(span, needle);
    }

    public static IndexEnumerable<T> EnumerateIndicesOf<T>(this Span<T> span, T needle)
    {
        return new(span, needle);
    }

    public static IndexEnumerable<char> EnumerateIndicesOf(this string? s, char needle)
    {
        return new(s, needle);
    }

    public static int SkipAndIndexOf<T>(this ReadOnlySpan<T> span, int skip, T needle)
    {
        var haystack = span.Slice(skip);
        var localIndex = haystack.IndexOf(needle);
        var result = localIndex == -1 ? -1 : skip + localIndex;
        return result;
    }

    public static int SkipAndIndexOf<T>(this Span<T> span, int skip, T needle)
    {
        return SkipAndIndexOf((ReadOnlySpan<T>)span, skip, needle);
    }

    public static T PopOrCreate<T>(this Stack<T> stack, Func<T> factory)
    {
        if (!stack.TryPop(out var result))
            result = factory.Invoke();
        return result;
    }

    public static T PopOrCreate<T>(this Stack<T> stack) where T : new()
    {
        if (!stack.TryPop(out var result))
            result = new();
        return result;
    }

    public static void PushRange<T>(this Stack<T> stack, List<T>? items)
    {
        stack.PushRange(CollectionsMarshal.AsSpan(items));
    }

    public static void PushRange<T>(this Stack<T> stack, params ReadOnlySpan<T> items)
    {
        foreach (var item in items)
            stack.Push(item);
    }

    public static TValue AlwaysGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
        where TValue : class, new()
    {
        if (!dictionary.TryGetValue(key, out var result))
        {
            result = new TValue();
            dictionary.Add(key, result);
        }
        else if (result is null)
        {
            result = new TValue();
            dictionary[key] = result;
        }

        return result;
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> factory
        ) where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = factory.Invoke(key);
            dictionary.Add(key, value);
        }

        return value;
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue
        ) where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = defaultValue;
            dictionary.Add(key, value);
        }

        return value;
    }

    public static bool IsValid(this nint ptr) => !IsInvalid(ptr);

    public static bool IsInvalid(this nint ptr)
    {
        if (Environment.Is64BitProcess)
        {
            var n = ptr.ToInt64();
            return -1 <= n && n <= 3;
        }
        else
        {
            var n = ptr.ToInt32();
            return -1 <= n && n <= 3;
        }
    }

    public unsafe static ReadOnlySpan<T> ToReadOnlySpan<T>(this nint ptr, int length) where T : unmanaged
    {
        var result = new ReadOnlySpan<T>(ptr.ToPointer(), length);
        return result;
    }

    public unsafe static Span<T> ToSpan<T>(this nint ptr, int length) where T : unmanaged
    {
        var result = new Span<T>(ptr.ToPointer(), length);
        return result;
    }

    public static Span<byte> NullTerminated(this Span<byte> span)
    {
        var index = span.IndexOf(default(byte));
        return index == -1 ? span : span.Slice(0, index);
    }

    public static ReadOnlySpan<byte> NullTerminated(this ReadOnlySpan<byte> span)
    {
        var index = span.IndexOf(default(byte));
        return index == -1 ? span : span.Slice(0, index);
    }

    public static void Fill<T>(this Span<T> span, Func<int, T> factory)
    {
        for (int i = 0; i < span.Length; ++i)
            span[i] = factory.Invoke(i);
    }

    public static void Fill<T, TState>(this Span<T> span, TState state, Func<int, TState, T> factory)
    {
        for (int i = 0; i < span.Length; ++i)
            span[i] = factory.Invoke(i, state);
    }

    public static void MutateAll<T, TState>(this Span<T> span, TState state, Func<TState, T, T> mutator)
    {
        for (int i = 0; i < span.Length; ++i)
            span[i] = mutator.Invoke(state, span[i]);
    }

    public static int Increment<TKey>(this IDictionary<TKey, int> dictionary, TKey key)
    {
        var nextValue = 1;
        if (dictionary.TryGetValue(key, out var value))
            nextValue = value + 1;

        dictionary[key] = nextValue;

        return nextValue;
    }

    public static long Increment<TKey>(this IDictionary<TKey, long> dictionary, TKey key)
    {
        var nextValue = 1L;
        if (dictionary.TryGetValue(key, out var value))
            nextValue = value + 1;

        dictionary[key] = nextValue;

        return nextValue;
    }

    public static ImmutableArray<TResult> ToImmutableArray<T, TResult>(
        this ImmutableArray<T> array,
        Func<T, TResult> conversion)
    {
        ArgumentNullException.ThrowIfNull(conversion);
        if (array.IsDefault)
            return default;
        if (array.IsEmpty)
            return [];
        var result = new TResult[array.Length];
        for (int i = 0; i < array.Length; ++i)
            result[i] = conversion.Invoke(array[i]);
        return ImmutableCollectionsMarshal.AsImmutableArray(result);
    }

    public static ImmutableArray<T> OrEmpty<T>(this ImmutableArray<T> array) => array.IsDefault ? [] : array;

    public static TResult[] ToArray<T, TResult>(this Span<T> span, Func<T, TResult> conversion)
    {
        var result = new TResult[span.Length];
        for (int i = 0; i < span.Length; ++i)
            result[i] = conversion.Invoke(span[i]);
        return result;
    }

    public static TResult[] ToArray<T, TResult>(this ReadOnlySpan<T> span, Func<T, TResult> conversion)
    {
        var result = new TResult[span.Length];
        for (int i = 0; i < span.Length; ++i)
            result[i] = conversion.Invoke(span[i]);
        return result;
    }

    public static ReadOnlySpan<T> After<T>(
        this ReadOnlySpan<T> span,
        ReadOnlySpan<T> value
        ) where T : IEquatable<T>
    {
        var result = span.StartsWith(value) ? span[value.Length..] : span;
        return result;
    }

    public static bool StartsWith<T>(
        this ReadOnlySpan<T> span,
        ReadOnlySpan<T> value,
        out ReadOnlySpan<T> after
        ) where T : IEquatable<T>
    {
        if (span.StartsWith(value))
        {
            after = span[value.Length..];
            return true;
        }
        else
        {
            after = default;
            return false;
        }
    }

    public static bool StartsWith(
        this string text,
        ReadOnlySpan<char> value,
        out ReadOnlySpan<char> after)
    {
        ArgumentNullException.ThrowIfNull(text);
        return text.AsSpan().StartsWith(value, out after);
    }

    public static int ByteSize<T>(
        this ReadOnlySpan<T> span
    ) where T : unmanaged
    {
        return span.Length * Unsafe.SizeOf<T>();
    }

    public static int ByteSize<T>(
        this Span<T> span
    ) where T : unmanaged
    {
        return span.Length * Unsafe.SizeOf<T>();
    }

    public static int ByteSize<T>(
        this T[]? array
    ) where T : unmanaged
    {
        return array.AsSpan().ByteSize();
    }

    public static int HalfStablePartition<T, TState>(
        this Span<T> span,
        TState state,
        Func<TState, T, bool> predicate) where TState : allows ref struct
    {
        int count = 0;

        while (count < span.Length && predicate.Invoke(state, span[count]))
            ++count;

        for (int i = count + 1; i < span.Length; ++i)
        {
            if (predicate.Invoke(state, span[i]))
            {
                var swapValue = span[count];
                span[count++] = span[i];
                span[i] = swapValue;
            }
        }

        return count;
    }

    public static int RemoveAll<T, TState>(
        this List<T> list,
        TState state,
        Func<T, TState, bool> predicate) where TState : allows ref struct
    {
        var span = CollectionsMarshal.AsSpan(list);
        int count = 0;

        while (count < span.Length && predicate.Invoke(span[count], state))
            ++count;

        for (int i = count + 1; i < span.Length; ++i)
        {
            if (predicate.Invoke(span[i], state))
                span[count++] = span[i];
        }

        list.RemoveRange(count, list.Count - count);

        return count;
    }
}

public readonly ref struct SpanWithIndexEnumerable<T>
{
    private readonly ReadOnlySpan<T> _span;
    public SpanWithIndexEnumerable(ReadOnlySpan<T> span) => _span = span;
    public SpanWithIndexEnumerator<T> GetEnumerator() => new(_span);
}

public ref struct SpanWithIndexEnumerator<T>
{
    private readonly ReadOnlySpan<T> _span;
    private int _index;
    public SpanWithIndexEnumerator(ReadOnlySpan<T> span)
    {
        _span = span;
        _index = -1;
    }

    public bool MoveNext() => ++_index < _span.Length;
    public readonly KeyValuePair<int, T> Current => new(_index, _span[_index]);
}

public readonly ref struct IndexEnumerable<T>
{
    private readonly ReadOnlySpan<T> _span;
    private readonly T _needle;
    public IndexEnumerable(ReadOnlySpan<T> span, T needle)
    {
        _span = span;
        _needle = needle;
    }
    public IndexEnumerator<T> GetEnumerator() => new(_span, _needle);
}

public ref struct IndexEnumerator<T>
{
    private readonly ReadOnlySpan<T> _span;
    private readonly T _needle;
    private int _next;
    public IndexEnumerator(ReadOnlySpan<T> span, T needle)
    {
        _span = span;
        _needle = needle;
    }
    public readonly int Current => _next - 1;
    public bool MoveNext()
    {
        var haystack = _span.Slice(_next);
        var index = haystack.IndexOf(_needle);
        if (index == -1)
            return false;
        _next += index + 1;
        return true;
    }
}
