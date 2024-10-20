using System;

namespace Piranha.Jawbone;

public struct Disposable : IDisposable
{
    private readonly Action? _action;

    public Disposable(Action? action) => _action = action;
    public void Dispose() => _action?.Invoke();
}
