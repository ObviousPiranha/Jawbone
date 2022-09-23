using System;

namespace Piranha.Jawbone.Collections;

public interface IUnmanagedList
{
    bool IsEmpty { get; }
    int Capacity { get; }
    int Count { get; }
    Span<byte> Bytes { get; }
}
