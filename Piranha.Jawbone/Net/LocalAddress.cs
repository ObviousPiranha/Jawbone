using System.ComponentModel;

namespace Piranha.Jawbone.Net;

public readonly struct LocalAddress
{
    public static LocalEndpoint OnAnyPort => default;
    public static LocalEndpoint OnPort(int port) => new() { Port = port };
    public static implicit operator Address32(LocalAddress _) => Address32.Local;
    public static implicit operator Address128(LocalAddress _) => Address128.Local;
}
