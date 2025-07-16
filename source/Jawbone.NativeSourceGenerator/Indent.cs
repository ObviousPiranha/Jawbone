using System;
using System.Text;

namespace Jawbone.NativeSourceGenerator;

readonly struct Indent
{
    public int Size { get; }
    public int Count { get; }

    public Indent(int size, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(size);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        Size = size;
        Count = count;
    }

    public Indent Increment() => new(Size, Count + 1);
}

static class IndentExtensions
{
    public static StringBuilder AppendIndent(this StringBuilder builder, Indent indent)
    {
        return builder.Append(' ', indent.Size * indent.Count);
    }
}
