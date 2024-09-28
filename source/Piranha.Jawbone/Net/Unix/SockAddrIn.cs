using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Unix;

struct SockAddrIn
{
    [InlineArray(Length)]
    public struct Zero
    {
        public const int Length = 8;
        private byte _e0;
    }

    public ushort SinFamily;
    public ushort SinPort;
    public uint SinAddr;
    public Zero SinZero;
}
