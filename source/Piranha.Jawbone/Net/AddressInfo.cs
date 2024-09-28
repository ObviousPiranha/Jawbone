using Piranha.Jawbone.Net.Unix;
using Piranha.Jawbone.Net.Windows;
using System;
using System.Collections.Immutable;

namespace Piranha.Jawbone.Net;

public readonly struct AddressInfo
{
    private const int BufferSize = 64;

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
        {
            return WindowsAddressInfo.Get(node, service, timeProvider);
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            return UnixAddressInfo.Get(node, service, timeProvider);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
