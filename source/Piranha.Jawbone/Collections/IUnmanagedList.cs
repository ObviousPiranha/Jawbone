using System;

namespace Piranha.Jawbone;

public interface IUnmanagedList
{
    bool IsEmpty { get; }
    int Capacity { get; }
    int Count { get; }
    int Size { get; }
    Span<byte> Bytes { get; }
}
