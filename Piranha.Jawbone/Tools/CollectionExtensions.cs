using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Tools.CollectionExtensions;

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
    public KeyValuePair<int, T> Current => KeyValuePair.Create(_index, _array[_index]);
}

public readonly struct ArrayWithIndexEnumerable<T>
{
    private readonly T[] _array;
    public ArrayWithIndexEnumerable(T[] array) => _array = array;
    public ArrayWithIndexEnumerator<T> GetEnumerator() => new ArrayWithIndexEnumerator<T>(_array);
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

    public static ReadOnlySpan<T> ToReadOnlySpan<T>(this IntPtr ptr, int length)
    {
        unsafe
        {
            var result = new ReadOnlySpan<T>(ptr.ToPointer(), length);
            return result;
        }
    }
}
