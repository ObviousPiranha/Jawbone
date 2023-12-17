using System;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress<TAddress> : IEquatable<TAddress>, ISpanParsable<TAddress> where TAddress : unmanaged, IAddress<TAddress>
{
    bool IsDefault { get; }
    bool IsLinkLocal { get; }
    bool IsLoopback { get; }
    void AppendTo(StringBuilder builder);

    static abstract TAddress Local { get; }
    static abstract Span<byte> AsBytes(ref TAddress address);
    static abstract ReadOnlySpan<byte> AsReadOnlyBytes(ref readonly TAddress address);
}
