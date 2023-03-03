using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net;

public readonly struct AddressInfo
{
    public readonly ImmutableArray<Endpoint<Address32>> V4 { get; init; }
    public readonly ImmutableArray<Endpoint<Address128>> V6 { get; init; }

    public static AddressInfo Get(string? node, string? service)
    {
        var v4 = ArrayPool<Endpoint<Address32>>.Shared.Rent(64);
        var v6 = ArrayPool<Endpoint<Address128>>.Shared.Rent(64);

        var error = JawboneNetworking.GetAddressInfo(
            node,
            service,
            out v4[0],
            Unsafe.SizeOf<Endpoint<Address32>>(),
            v4.Length,
            out var countV4,
            out v6[0],
            Unsafe.SizeOf<Endpoint<Address128>>(),
            v6.Length,
            out var countV6);

        SocketException.ThrowOnError(error, "Unable to get address info.");

        var result = new AddressInfo
        {
            V4 = ImmutableArray.Create<Endpoint<Address32>>(v4, 0, countV4),
            V6 = ImmutableArray.Create<Endpoint<Address128>>(v6, 0, countV6)
        };

        ArrayPool<Endpoint<Address32>>.Shared.Return(v4);
        ArrayPool<Endpoint<Address128>>.Shared.Return(v6);

        return result;
    }
}
