using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Extensions;

public struct ArrayWithIndexEnumerator<T>
{
    private readonly T[] _array;
    private int _index;
    public ArrayWithIndexEnumerator(T[] array)
    {
        _array = array;
        _index = -1;
    }

    public bool MoveNext() => ++_index < _array.Length;
    public readonly KeyValuePair<int, T> Current => new(_index, _array[_index]);
}

public readonly struct ArrayWithIndexEnumerable<T>
{
    private readonly T[] _array;
    public ArrayWithIndexEnumerable(T[] array) => _array = array;
    public ArrayWithIndexEnumerator<T> GetEnumerator() => new(_array);
}

public static class CollectionExtensions
{
    public static ArrayWithIndexEnumerable<T> WithIndex<T>(this T[] array)
    {
        return new ArrayWithIndexEnumerable<T>(array);
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

    public static string? AsCString(this IntPtr ptr)
    {
        return ptr.IsInvalid() ? null : Marshal.PtrToStringUTF8(ptr);
    }

    public static bool IsValid(this IntPtr ptr) => !IsInvalid(ptr);

    public static bool IsInvalid(this IntPtr ptr)
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

    public unsafe static ReadOnlySpan<T> ToReadOnlySpan<T>(this IntPtr ptr, int length) where T : unmanaged
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
}
