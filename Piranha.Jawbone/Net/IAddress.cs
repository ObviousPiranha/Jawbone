using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress<TAddress> : IEquatable<TAddress>, ISpanParsable<TAddress> where TAddress : unmanaged, IAddress<TAddress>
{
    bool IsDefault { get; }
    bool IsLinkLocal { get; }
    bool IsLoopback { get; }
    void AppendTo(StringBuilder builder);

    static abstract TAddress Any { get; }
    static abstract TAddress Local { get; }
    static abstract Span<byte> GetBytes(ref TAddress address);
    static abstract ReadOnlySpan<byte> GetReadOnlyBytes(in TAddress address);
    static virtual implicit operator TAddress(AnyAddress anyAddress) => TAddress.Any;
    static virtual implicit operator TAddress(LocalAddress localAddress) => TAddress.Local;
}
