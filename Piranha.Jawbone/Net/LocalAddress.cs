namespace Piranha.Jawbone.Net;

public readonly struct LocalAddress
{
    public static LocalEndpoint OnAnyPort() => default;
    public static LocalEndpoint OnPort(int port) => new() { Port = port };
}
