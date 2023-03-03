using System;

namespace Piranha.Jawbone.Sdl;

public readonly struct WindowEventView
{
    private readonly byte[] _data;

    public uint Timestamp => BitConverter.ToUInt32(_data, 4);
    public uint WindowId => BitConverter.ToUInt32(_data, 8);
    public byte Event => _data[12];
    public int X => BitConverter.ToInt32(_data, 16);
    public int Y => BitConverter.ToInt32(_data, 20);

    public WindowEventView(byte[] data) => _data = data;
}
