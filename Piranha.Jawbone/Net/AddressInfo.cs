using System.Collections.Immutable;

namespace Piranha.Jawbone.Net;

public readonly struct AddressInfo
{
    public readonly ImmutableArray<Endpoint<Address32>> V4 { get; init; }
    public readonly ImmutableArray<Endpoint<Address128>> V6 { get; init; }
}