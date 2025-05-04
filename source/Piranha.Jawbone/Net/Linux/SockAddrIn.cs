using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Linux;

struct SockAddrIn
{
    [InlineArray(8)]
    public struct Zero
    {
        private byte _e0;
    }

    public ushort SinFamily;
    public ushort SinPort;
    public uint SinAddr;
    public Zero SinZero;

    public Endpoint<AddressV4> ToEndpoint(uint len)
    {
        if (len != Len)
            Core.ThrowBadAddressLength();
        return ToEndpoint();
    }

    public Endpoint<AddressV4> ToEndpoint()
    {
        if (SinFamily != Af.INet)
            Core.ThrowWrongAddressFamily();
        return Endpoint.Create(
            new AddressV4(SinAddr),
            new NetworkPort { NetworkValue = SinPort });
    }

    public static uint Len => Sys.SockLen<SockAddrIn>();

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
