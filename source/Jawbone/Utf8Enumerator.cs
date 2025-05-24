using System;

namespace Jawbone;

public ref struct Utf8Enumerator
{
    private readonly ReadOnlySpan<byte> _bytes;
    private int _index;

    public int Current { get; private set; }

    public Utf8Enumerator(ReadOnlySpan<byte> bytes) => _bytes = bytes;

    public bool MoveNext()
    {
        if (_index == _bytes.Length)
        {
            Current = 0;
            return false;
        }

        (Current, var length) = Utf8.ReadCodePoint(_bytes[_index..]);
        _index += length;
        return true;
    }
}
