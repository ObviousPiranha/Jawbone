using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress<TAddress> : IEquatable<TAddress> where TAddress : unmanaged
{
    bool IsDefault { get; }
    bool IsLinkLocal { get; }
    void AppendTo(StringBuilder builder);

    static abstract TAddress Any { get; }
    static abstract TAddress Local { get; }
    static abstract Span<byte> GetBytes(ref TAddress address);
    static abstract ReadOnlySpan<byte> GetReadOnlyBytes(in TAddress address);
}
