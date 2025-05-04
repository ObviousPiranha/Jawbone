using Piranha.Jawbone.Net.Linux;
using Piranha.Jawbone.Net.Mac;
using Piranha.Jawbone.Net.Windows;
using System;
using System.Collections.Immutable;

namespace Piranha.Jawbone.Net;

public readonly struct AddressInfo
{
    public DateTimeOffset CreatedAt { get; init; }
    public readonly string? Node { get; init; }
    public readonly string? Service { get; init; }
    public readonly ImmutableArray<Endpoint<AddressV4>> V4 { get; init; }
    public readonly ImmutableArray<Endpoint<AddressV6>> V6 { get; init; }

    public readonly bool IsEmpty => V4.IsDefaultOrEmpty && V6.IsDefaultOrEmpty;

    public static AddressInfo Get(
        string? node,
        string? service = null,
        TimeProvider? timeProvider = null)
    {
        if (OperatingSystem.IsWindows())
            return WindowsAddressInfo.Get(node, service, timeProvider);
        if (OperatingSystem.IsMacOS())
            return MacAddressInfo.Get(node, service, timeProvider);
        if (OperatingSystem.IsLinux())
            return LinuxAddressInfo.Get(node, service, timeProvider);
        throw new PlatformNotSupportedException();
    }
}
