using System;

namespace Piranha.Jawbone.Sdl;

public readonly ref struct TextInputEventView
{
    private readonly byte[] _data;

    public uint Timestamp => BitConverter.ToUInt32(_data, 4);
    public uint WindowId => BitConverter.ToUInt32(_data, 8);
    public ReadOnlySpan<byte> Text => _data.AsSpan(12, 32);
    public TextInputEventView(byte[] data) => _data = data;
}