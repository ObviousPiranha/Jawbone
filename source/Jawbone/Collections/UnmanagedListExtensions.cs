using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Jawbone;

public static class UnmanagedListExtensions
{
    public static bool StartsWith<T>(
        this UnmanagedList<T> list,
        T item
        ) where T : unmanaged, IEquatable<T>
    {
        return !list.IsEmpty && list[0].Equals(item);
    }

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

    public static UnmanagedList<byte> AppendUtf8CodePoint(
        this UnmanagedList<byte> list,
        int codePoint)
    {
        if (codePoint < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(codePoint), "Negative code points not allowed.");
        }
        else if (codePoint < 0x80)
        {
            list.Add((byte)codePoint);
        }
        else if (codePoint < 0x800)
        {
            list.Add(
                (byte)(0xc0 | (codePoint >> 6)),
                (byte)Utf8.GetContinuationByte(codePoint, 0));
        }
        else if (codePoint < 0x10000)
        {
            list.Add(
                (byte)(0xe0 | (codePoint >> 12)),
                (byte)Utf8.GetContinuationByte(codePoint, 1),
                (byte)Utf8.GetContinuationByte(codePoint, 0));
        }
        else if (codePoint < 0x110000)
        {
            list.Add(
                (byte)(0xf0 | (codePoint >> 18)),
                (byte)Utf8.GetContinuationByte(codePoint, 2),
                (byte)Utf8.GetContinuationByte(codePoint, 1),
                (byte)Utf8.GetContinuationByte(codePoint, 0));
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(codePoint), $"Invalid code point {codePoint}");
        }

        return list;
    }

    public static UnmanagedList<byte> AppendUtf8(
        this UnmanagedList<byte> list,
        ReadOnlySpan<char> utf16)
    {
        for (int i = 0; i < utf16.Length; ++i)
        {
            var c = utf16[i];
            if (char.IsSurrogate(c))
            {
                list.AppendUtf8CodePoint(char.ConvertToUtf32(c, utf16[++i]));
            }
            else
            {
                list.AppendUtf8CodePoint(c);
            }
        }

        return list;
    }

    public static UnmanagedList<byte> AppendUtf8(
        this UnmanagedList<byte> list,
        ReadOnlySpan<int> utf32)
    {
        foreach (var codePoint in utf32)
            list.AppendUtf8CodePoint(codePoint);

        return list;
    }

    public static UnmanagedList<int> AppendUtf32(
        this UnmanagedList<int> list,
        ReadOnlySpan<char> utf16)
    {
        for (int i = 0; i < utf16.Length; ++i)
        {
            var c = utf16[i];
            if (char.IsSurrogate(c))
            {
                list.Add(char.ConvertToUtf32(c, utf16[++i]));
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

    public static UnmanagedList<char> AppendUtf32(
        this UnmanagedList<char> list,
        int utf32)
    {
        var rune = new Rune(utf32);
        var buffer = list.AcquireUninitialized(2);
        var n = rune.EncodeToUtf16(buffer);
        if (n == 1)
            list.RemoveLast();
        else if (n != 2) // lolwut how
            Throw();
        return list;
        static void Throw() => throw new InvalidOperationException("UTF-16 encode failed.");
    }

    public static UnmanagedList<char> InsertUtf32(
        this UnmanagedList<char> list,
        ref int index,
        int utf32)
    {
        Span<char> buffer = stackalloc char[2];
        var rune = new Rune(utf32);
        var n = rune.EncodeToUtf16(buffer);
        list.InsertAll(index, buffer[..n]);
        index += n;
        return list;
    }

    public static UnmanagedList<char> RemoveCodePointBefore(
        this UnmanagedList<char> list,
        ref int index)
    {
        --index;
        try
        {
            return list.RemoveCodePointAt(ref index);
        }
        catch
        {
            ++index;
            throw;
        }
    }

    public static UnmanagedList<char> RemoveCodePointAt(
        this UnmanagedList<char> list,
        ref int index)
    {
        if (char.IsHighSurrogate(list[index]))
        {
            var neighbor = list[index + 1];
            if (!char.IsLowSurrogate(neighbor))
                throw new InvalidOperationException($"Missing low surrogate at index {index + 1}.");
            list.RemoveAt(index, 2);
        }
        else if (char.IsLowSurrogate(list[index]))
        {
            var neighbor = list[index - 1];
            if (!char.IsHighSurrogate(neighbor))
                throw new InvalidOperationException($"Missing high surrogate at index {index - 1}.");
            list.RemoveAt(index - 1, 2);
            --index;
        }
        else
        {
            list.RemoveAt(index);
        }

        return list;
    }

    public static string AsUtf32String(this UnmanagedList<int> list)
    {
        var result = Encoding.UTF32.GetString(list.Bytes);
        return result;
    }

    public static string AsUtf8String(this UnmanagedList<byte> list)
    {
        var result = Encoding.UTF8.GetString(list.AsSpan());
        return result;
    }
}
