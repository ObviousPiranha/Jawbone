using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Jawbone;

public readonly struct CStringArray : IDisposable
{
    private readonly nint _allocated;

    public nint Pointer { get; }
    public int Length { get; }

    public CStringArray(ReadOnlySpan<string> array)
    {
        if (array.IsEmpty)
            return;
        Length = array.Length;

        var pointerArraySize = Length * nint.Size;
        var encoding = Encoding.UTF8;
        var stringsSize = Length; //  Account for null terminators.
        foreach (var item in array)
            stringsSize += encoding.GetByteCount(item);
        Pointer = _allocated = Marshal.AllocHGlobal(pointerArraySize + stringsSize);
        var pointerWriter = SpanWriter.Create<nint>(Pointer, Length);
        var stringWriter = SpanWriter.Create<byte>(Pointer + pointerArraySize, stringsSize);
        foreach (var item in array)
        {
            pointerWriter.Write(Pointer + pointerArraySize + stringWriter.Position);
            var tryWriteResult = stringWriter.TryWriteUtf8(item);
            Debug.Assert(tryWriteResult);
            Debug.Assert(!stringWriter.IsFull);
            stringWriter.Write(byte.MinValue);
        }
        Debug.Assert(stringWriter.IsFull);
    }

    public CStringArray(nint pointer, int length)
    {
        Pointer = pointer;
        Length = length;
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(_allocated);
    }

    public IEnumerable<string?> Enumerate()
    {
        for (int i = 0; i < Length; ++i)
        {
            var ptr = Marshal.ReadIntPtr(Pointer + i * nint.Size);
            var result = Marshal.PtrToStringUTF8(ptr);
            yield return result;
        }
    }

    public static CStringArray FromCommandLine() => new(Environment.GetCommandLineArgs());
}
