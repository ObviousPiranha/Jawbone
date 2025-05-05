using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Mac;

struct SockAddrIn
{
    [InlineArray(8)]
    public struct Zero
    {
        private byte _e0;
    }

    // https://stackoverflow.com/a/41580692/264712
    public byte SinLen;
    public byte SinFamily;
    public ushort SinPort;
    public uint SinAddr;
    public Zero SinZero;

    public Endpoint<AddressV4> ToEndpoint(uint len)
    {
        if (len != Len)
            ThrowExceptionFor.WrongAddressLength();
        return ToEndpoint();
    }

    public Endpoint<AddressV4> ToEndpoint()
    {
        if (SinFamily != Af.INet)
            ThrowExceptionFor.WrongAddressFamily();
        return Endpoint.Create(
            new AddressV4(SinAddr),
            new NetworkPort { NetworkValue = SinPort });
    }

    public static uint Len => Sys.SockLen<SockAddrIn>();

    public static SockAddrIn FromEndpoint(Endpoint<AddressV4> endpoint)
    {
        return new SockAddrIn
        {
            SinLen = (byte)Unsafe.SizeOf<SockAddrIn>(),
            SinFamily = Af.INet,
            SinPort = endpoint.Port.NetworkValue,
            SinAddr = endpoint.Address.DataU32
        };
    }
}
