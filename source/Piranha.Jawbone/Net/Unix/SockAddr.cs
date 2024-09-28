using System.Runtime.CompilerServices;

struct SockAddr
{
    [InlineArray(Length)]
    public struct Data
    {
        public const int Length = 14;
        private byte _e0;
    }

    public ushort SaFamily;
    public Data SaData;
}
