using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net;

public readonly struct AddressInfo
{
    public readonly ImmutableArray<Endpoint<AddressV4>> V4 { get; init; }
    public readonly ImmutableArray<Endpoint<AddressV6>> V6 { get; init; }

    public static AddressInfo Get(string? node, string? service = null)
    {
        var v4 = ArrayPool<Endpoint<AddressV4>>.Shared.Rent(64);
        var v6 = ArrayPool<Endpoint<AddressV6>>.Shared.Rent(64);

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

        var result = new AddressInfo
        {
            V4 = ImmutableArray.Create(v4, 0, countV4),
            V6 = ImmutableArray.Create(v6, 0, countV6)
        };

        ArrayPool<Endpoint<AddressV4>>.Shared.Return(v4);
        ArrayPool<Endpoint<AddressV6>>.Shared.Return(v6);

        return result;
    }
}
