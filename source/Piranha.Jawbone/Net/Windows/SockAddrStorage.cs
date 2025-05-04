using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net.Windows;

[StructLayout(LayoutKind.Explicit, Size = 128)]
struct SockAddrStorage
{
    [FieldOffset(0)]
    public SockAddrIn V4;
    [FieldOffset(0)]
    public SockAddrIn6 V6;

    public Endpoint<AddressV6> GetV6(int addrLen)
    {
        if (addrLen == SockAddrIn6.Len)
            return V6.ToEndpoint();
        else if (addrLen == SockAddrIn.Len)
            return V4.ToEndpoint().MapToV6();
        else
            throw new SocketException("Unsupported address size: " + addrLen);
    }

    public static int Len => Unsafe.SizeOf<SockAddrStorage>();
}
