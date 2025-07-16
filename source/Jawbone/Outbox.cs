using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Jawbone;

public sealed class Outbox<T> : IDisposable
{
    private T _value;
    private int _taken;

    public bool Taken => _taken != 0;

    public Outbox(T value) => _value = value;

    public T Take()
    {
        if (!TryTakeFlag(1))
            throw new InvalidOperationException("Value was already taken.");
        var result = _value;
        _value = default!;
        return result;
    }

    public bool TryTake([MaybeNullWhen(false)] out T value)
    {
        if (TryTakeFlag(2))
        {
            value = _value;
            _value = default!;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    private bool TryTakeFlag(int flagValue)
    {
        var oldTaken = Interlocked.CompareExchange(ref _taken, flagValue, 0);
        return oldTaken == 0;
    }

    public void Dispose()
    {
        if (TryTakeFlag(3))
            _value = default!;
    }
}
