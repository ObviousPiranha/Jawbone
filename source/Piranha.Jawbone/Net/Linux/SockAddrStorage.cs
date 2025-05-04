using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net.Linux;

[StructLayout(LayoutKind.Explicit)]
struct SockAddrStorage
{
    [FieldOffset(0)]
    public ushort Family;
    [FieldOffset(0)]
    public SockAddrIn V4;
    [FieldOffset(0)]
    public SockAddrIn6 V6;

    public Endpoint<AddressV6> GetV6(uint addrLen)
    {
        if (addrLen == SockAddrIn6.Len)
            return V6.ToEndpoint();
        else if (addrLen == SockAddrIn.Len)
            return V4.ToEndpoint().MapToV6();
        else
            throw new InvalidOperationException("Unsupported address size: " + addrLen);
    }
}
