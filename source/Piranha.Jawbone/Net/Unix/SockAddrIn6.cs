namespace Piranha.Jawbone.Net.Unix;

struct SockAddrIn6
{
    public ushort Sin6Family;
    public ushort Sin6Port;
    public uint Sin6FlowInfo;
    public In6Addr Sin6Addr;
    public uint Sin6ScopeId;
}
