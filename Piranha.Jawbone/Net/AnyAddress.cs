using System.ComponentModel;

namespace Piranha.Jawbone.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct AnyAddress
{
    public static AnyEndpoint OnAnyPort => default;
    public static AnyEndpoint OnPort(int port) => new() { Port = port };
    public static implicit operator Address32(AnyAddress _) => Address32.Any;
    public static implicit operator Address128(AnyAddress _) => Address128.Any;
}
