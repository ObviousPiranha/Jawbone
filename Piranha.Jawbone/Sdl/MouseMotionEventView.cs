using System;

namespace Piranha.Jawbone.Sdl
{
    public readonly struct MouseMotionEventView
    {
        private readonly byte[] _data;

        public uint Timestamp => BitConverter.ToUInt32(_data, 4);
        public uint WindowId => BitConverter.ToUInt32(_data, 8);
        public uint Which => BitConverter.ToUInt32(_data, 12);
        public uint State => BitConverter.ToUInt32(_data, 16);
        public int X => BitConverter.ToInt32(_data, 20);
        public int Y => BitConverter.ToInt32(_data, 24);
        public int RelativeX => BitConverter.ToInt32(_data, 28);
        public int RelativeY => BitConverter.ToInt32(_data, 32);

        public MouseMotionEventView(byte[] data)
        {
            _data = data;
        }
    }
}
