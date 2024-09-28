using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

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

    public Endpoint<AddressV4> ToEndpoint()
    {
        return Endpoint.Create(
            new AddressV4(SinAddr),
            new NetworkPort { NetworkValue = SinPort });
    }

    public static SockAddrIn FromEndpoint(Endpoint<AddressV4> endpoint)
    {
        return new SockAddrIn
        {
            SinFamily = Af.INet,
            SinPort = endpoint.Port.NetworkValue,
            SinAddr = endpoint.Address.DataU32
        };
    }
}
