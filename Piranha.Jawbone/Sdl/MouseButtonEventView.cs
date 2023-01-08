using System;

namespace Piranha.Jawbone.Sdl;

public readonly ref struct MouseButtonEventView
{
    private readonly byte[] _data;

    public uint Timestamp => BitConverter.ToUInt32(_data, 4);
    public uint WindowId => BitConverter.ToUInt32(_data, 8);
    public uint Which => BitConverter.ToUInt32(_data, 12);
    public byte Button => _data[16];
    public byte State => _data[17];
    public byte Clicks => _data[18];
    public int X => BitConverter.ToInt32(_data, 20);
    public int Y => BitConverter.ToInt32(_data, 24);

    public MouseButtonEventView(byte[] data)
    {
        _data = data;
    }
}
