using System;

namespace Jawbone;

public readonly struct Disposable : IDisposable
{
    private readonly Action? _action;

    public Disposable(Action? action) => _action = action;
    public readonly void Dispose() => _action?.Invoke();
}
