using System;
using System.Collections.Generic;
using System.Text;

namespace Piranha.Jawbone;

public static class UnmanagedListExtensions
{
    public static UnmanagedList<T> Append<T>(
        this UnmanagedList<T> list,
        T item
        ) where T : unmanaged
    {
        list.Add(item);
        return list;
    }

    public static UnmanagedList<T> AppendAll<T>(
        this UnmanagedList<T> list,
        ReadOnlySpan<T> items
        ) where T : unmanaged
    {
        list.AddAll(items);
        return list;
    }

    public static UnmanagedList<T> AppendRange<T>(
        this UnmanagedList<T> list,
        IEnumerable<T> items
        ) where T : unmanaged
    {
        list.AddRange(items);
        return list;
    }

    public static UnmanagedList<T> AppendMany<T>(
        this UnmanagedList<T> list,
        params T[] items
        ) where T : unmanaged
    {
        list.AddAll(items);
        return list;
    }

    public static UnmanagedList<int> AppendUtf32(
        this UnmanagedList<int> list,
        ReadOnlySpan<char> text)
    {
        for (int i = 0; i < text.Length; ++i)
        {
            var c = text[i];
            if (char.IsSurrogate(c))
            {
                list.Add(char.ConvertToUtf32(c, text[++i]));
            }
            else
            {
                list.Add(c);
            }
        }

        return list;
    }

    public static UnmanagedList<int> AppendUtf32(
        this UnmanagedList<int> list,
        StringBuilder stringBuilder)
    {
        for (int i = 0; i < stringBuilder.Length; ++i)
        {
            var c = stringBuilder[i];
            if (char.IsSurrogate(c))
            {
                list.Add(char.ConvertToUtf32(c, stringBuilder[++i]));
            }
            else
            {
                list.Add(c);
            }
        }

        return list;
    }

    public static UnmanagedList<int> AppendUtf32(
        this UnmanagedList<int> list,
        uint value)
    {
        if (value == 0)
        {
            list.Add('0');
            return list;
        }

        var firstIndex = list.Count;

        for (var i = value; 0 < i; i /= 10)
            list.Add('0' + (int)(i % 10));

        list.AsSpan(firstIndex).Reverse();
        return list;
    }

    public static UnmanagedList<int> AppendUtf32(
        this UnmanagedList<int> list,
        int value)
    {
        if (value < 0)
        {
            list.Add('-');
            return list.AppendUtf32((uint)-value);
        }

        return list.AppendUtf32((uint)value);
    }
}
