namespace Piranha.Jawbone.Net;

public static class Address
{
    public static Endpoint<TAddress> OnAnyPort<TAddress>(
        this TAddress address
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, default(NetworkPort));
    }

    public static Endpoint<TAddress> OnPort<TAddress>(
        this TAddress address,
        int port
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, port);
    }

    public static Endpoint<TAddress> OnPort<TAddress>(
        this TAddress address,
        NetworkPort port
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return new(address, port);
    }
}
