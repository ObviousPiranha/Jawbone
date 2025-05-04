using System;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress
{
    bool IsDefault { get; }
    bool IsLinkLocal { get; }
    bool IsLoopback { get; }
    void AppendTo(StringBuilder builder);
}

public interface IAddress<TAddress> : IEquatable<TAddress>, ISpanParsable<TAddress>, IAddress
    where TAddress : unmanaged, IAddress<TAddress>
{
    static abstract TAddress Local { get; }
    static abstract Span<byte> AsBytes(ref TAddress address);
    static abstract ReadOnlySpan<byte> AsReadOnlyBytes(ref readonly TAddress address);
}
