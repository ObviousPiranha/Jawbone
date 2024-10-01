using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net.Mac;

[StructLayout(LayoutKind.Explicit)]
struct SockAddrStorage
{
    [FieldOffset(0)]
    public SockAddrIn A;
    [FieldOffset(0)]
    public SockAddrIn6 B;
}
