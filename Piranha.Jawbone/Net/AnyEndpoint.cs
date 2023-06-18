using System.ComponentModel;

namespace Piranha.Jawbone.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct AnyEndpoint
{
    public static implicit operator Endpoint<Address32>(AnyEndpoint _) => default;
    public static implicit operator Endpoint<Address128>(AnyEndpoint _) => default;
}