using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Jawbone;

public readonly struct CString : IUtf8SpanFormattable, ISpanFormattable
{
    public nint Address { get; }

    public CString(nint address) => Address = address;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public readonly string? GetStringOrDefault(string? defaultValue) => ToString() ?? defaultValue;
    public readonly string GetStringOrEmpty() => ToString() ?? "";
    public unsafe ReadOnlySpan<byte> AsSpan()
    {
        var pointer = (byte*)Address.ToPointer();
        var result = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(pointer);
        return result;
    }

    public override string? ToString() => Marshal.PtrToStringUTF8(Address);

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        var span = AsSpan();
        if (utf8Destination.Length < span.Length)
        {
            span[..utf8Destination.Length].CopyTo(utf8Destination);
            bytesWritten = utf8Destination.Length;
            return false;
        }
        else
        {
            span.CopyTo(utf8Destination);
            bytesWritten = span.Length;
            return true;
        }
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        var span = AsSpan();
        var result = Encoding.UTF8.TryGetChars(span, destination, out charsWritten);
        return result;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => GetStringOrEmpty();
}
