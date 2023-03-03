using System;

namespace Piranha.Jawbone.Sdl;

public readonly ref struct MouseWheelEventView
{
    private readonly byte[] _data;

    public uint Timestamp => BitConverter.ToUInt32(_data, 4);
    public uint WindowId => BitConverter.ToUInt32(_data, 8);
    public uint Which => BitConverter.ToUInt32(_data, 12);
    public int X => BitConverter.ToInt32(_data, 16);
    public int Y => BitConverter.ToInt32(_data, 20);
    public uint Direction => BitConverter.ToUInt32(_data, 24);

    public MouseWheelEventView(byte[] data)
    {
        _data = data;
    }
}
