using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

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
        var v4 = ArrayPool<Endpoint<AddressV4>>.Shared.Rent(BufferSize);
        var v6 = ArrayPool<Endpoint<AddressV6>>.Shared.Rent(BufferSize);

        try
        {
            var error = JawboneNetworking.GetAddressInfo(
                node,
                service,
                out v4[0],
                Unsafe.SizeOf<Endpoint<AddressV4>>(),
                v4.Length,
                out var countV4,
                out v6[0],
                Unsafe.SizeOf<Endpoint<AddressV6>>(),
                v6.Length,
                out var countV6);

            SocketException.ThrowOnError(error, "Unable to get address info.");

            timeProvider ??= TimeProvider.System;

            var result = new AddressInfo
            {
                CreatedAt = timeProvider.GetLocalNow(),
                Node = node,
                Service = service,
                V4 = ImmutableArray.Create(v4, 0, countV4),
                V6 = ImmutableArray.Create(v6, 0, countV6)
            };

            return result;
        }
        finally
        {
            ArrayPool<Endpoint<AddressV4>>.Shared.Return(v4);
            ArrayPool<Endpoint<AddressV6>>.Shared.Return(v6);
        }
    }
}
