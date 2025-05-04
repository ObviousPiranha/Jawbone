using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net.Linux;

[StructLayout(LayoutKind.Explicit)]
struct SockAddrStorage
{
    [FieldOffset(0)]
    public SockAddrIn V4;
    [FieldOffset(0)]
    public SockAddrIn6 V6;

    public Endpoint<AddressV6> GetV6(uint addrLen)
    {
        if (addrLen == Sys.SockLen<SockAddrIn>())
            return V4.ToEndpoint().MapToV6();
        else if (addrLen == Sys.SockLen<SockAddrIn6>())
            return V6.ToEndpoint();
        else
            throw new SocketException("Unsupported address size: " + addrLen);
    }
}
