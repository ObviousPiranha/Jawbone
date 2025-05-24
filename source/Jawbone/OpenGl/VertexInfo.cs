using System;

namespace Jawbone.OpenGl;

readonly struct VertexInfo
{
    public readonly CommonVertexInfo Common { get; init; }
    public readonly int Index { get; init; }
    public readonly uint Normalized { get; init; }
    public readonly int Offset { get; init; }
}
