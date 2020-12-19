using System;

namespace Piranha.Jawbone.Sdl
{
    public readonly ref struct UserEventView
    {
        private readonly byte[] _data;

        public uint EventType
        {
            get => BitConverter.ToUInt32(_data, 0);
            set => BitConverter.TryWriteBytes(_data, value);
        }

        public uint Timestamp
        {
            get => BitConverter.ToUInt32(_data, 4);
            set => BitConverter.TryWriteBytes(_data.AsSpan(4), value);
        }

        public uint WindowId
        {
            get => BitConverter.ToUInt32(_data, 8);
            set => BitConverter.TryWriteBytes(_data.AsSpan(8), value);
        }

        public int Code
        {
            get => BitConverter.ToInt32(_data, 12);
            set => BitConverter.TryWriteBytes(_data.AsSpan(12), value);
        }

        public IntPtr Data1
        {
            get
            {
                return Environment.Is64BitProcess ?
                    new IntPtr(BitConverter.ToInt64(_data, 16)) :
                    new IntPtr(BitConverter.ToInt32(_data, 16));
            }
            
            set
            {
                if (Environment.Is64BitProcess)
                    BitConverter.TryWriteBytes(_data.AsSpan(16), value.ToInt64());
                else
                    BitConverter.TryWriteBytes(_data.AsSpan(16), value.ToInt32());
            }
        }

        public IntPtr Data2
        {
            get
            {
                return Environment.Is64BitProcess ?
                    new IntPtr(BitConverter.ToInt64(_data, 24)) :
                    new IntPtr(BitConverter.ToInt32(_data, 20));
            }
            
            set
            {
                if (Environment.Is64BitProcess)
                    BitConverter.TryWriteBytes(_data.AsSpan(24), value.ToInt64());
                else
                    BitConverter.TryWriteBytes(_data.AsSpan(20), value.ToInt32());
            }
        }

        public UserEventView(byte[] data) => _data = data;
    }
}
