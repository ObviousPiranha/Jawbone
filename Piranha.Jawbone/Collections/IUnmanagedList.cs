using System;

namespace Piranha.Jawbone.Collections;

public interface IUnmanagedList
{
    bool IsEmpty => Count == 0;
    int Capacity { get; }
    int Count { get; }
    Span<byte> Bytes { get; }
}
