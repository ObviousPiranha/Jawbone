using System.Diagnostics;

namespace Jawbone;

sealed class UnmanagedListDebugView<T> where T : unmanaged
{
    private readonly UnmanagedList<T> _list;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items => _list.AsSpan().ToArray();

    public UnmanagedListDebugView(UnmanagedList<T> list) => _list = list;
}
