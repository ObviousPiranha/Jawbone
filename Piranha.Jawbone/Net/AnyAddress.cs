namespace Piranha.Jawbone.Net;

public readonly struct AnyAddress
{
    public static AnyEndpoint OnAnyPort() => default;
    public static AnyEndpoint OnPort(int port) => new() { Port = port };
}
