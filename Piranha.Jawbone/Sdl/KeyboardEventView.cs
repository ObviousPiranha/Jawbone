using System;

namespace Piranha.Jawbone.Sdl;

public readonly ref struct KeyboardEventView
{
    private readonly byte[] _data;

    public uint Timestamp => BitConverter.ToUInt32(_data, 4);
    public uint WindowId => BitConverter.ToUInt32(_data, 8);
    public byte State => _data[12];
    public byte Repeat => _data[13];
    public int PhysicalKeyCode => BitConverter.ToInt32(_data, 16);
    public int VirtualKeyCode => BitConverter.ToInt32(_data, 20);
    public ushort Mod => BitConverter.ToUInt16(_data, 24);

    public KeyboardEventView(byte[] data)
    {
        _data = data;
    }
}
