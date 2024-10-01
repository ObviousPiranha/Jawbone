using System;
using System.Collections.Immutable;

namespace Piranha.Jawbone.Net.Unix;

static class UnixAddressInfo
{
    public static unsafe AddressInfo Get(
        string? node,
        string? service = null,
        TimeProvider? timeProvider = null)
    {
        var hints = default(AddrInfo);
        hints.AiFamily = Sys.Select(Mac.Af.Unspec, Linux.Af.Unspec);
        var result = Sys.GetAddrInfo(node, service, hints, out var res);

        if (result == -1)
            Sys.Throw("Unable to get address info.");

        var v4 = ImmutableArray.CreateBuilder<Endpoint<AddressV4>>();
        var v6 = ImmutableArray.CreateBuilder<Endpoint<AddressV6>>();

        try
        {
            for (var ai = res; ai != null; ai = ai->AiNext)
            {
                if (ai->AiFamily == Sys.Select(Mac.Af.INet, Linux.Af.INet))
                {
                    var addr = (SockAddrIn*)ai->AiAddr;
                    var endpoint = addr->ToEndpoint();
                    v4.Add(endpoint);
                }
                if (ai->AiFamily == Sys.Select(Mac.Af.INet6, Linux.Af.INet6))
                {
                    var addr = (SockAddrIn6*)ai->AiAddr;
                    var endpoint = addr->ToEndpoint();
                    v6.Add(endpoint);
                }
            }
        }
        finally
        {
            Sys.FreeAddrInfo(res);
        }

        timeProvider ??= TimeProvider.System;

        return new AddressInfo
        {
            CreatedAt = timeProvider.GetLocalNow(),
            Node = node,
            Service = service,
            V4 = v4.DrainToImmutable(),
            V6 = v6.DrainToImmutable()
        };
    }
}
