using System;
using System.Text;

namespace Jawbone.SourceGenerator;

readonly struct IndentState
{
    public readonly int Size { get; }
    public readonly int Count { get; }
    public readonly int Total => Count * Size;

    public IndentState(int size, int count = 0)
    {
        Size = size;
        Count = count;
    }

    public readonly IndentState Indent() => new(Size, Count + 1);
}
