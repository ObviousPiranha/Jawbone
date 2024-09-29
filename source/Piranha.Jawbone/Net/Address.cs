namespace Piranha.Jawbone.Net;

public readonly struct Address
{
    private readonly AddressV6 _storage;

    public readonly AddressType Type { get; }

    public Address(AddressV4 address)
    {
        Type = AddressType.V4;
        _storage.DataU32[0] = address.DataU32;
    }

    public Address(AddressV6 address)
    {
        Type = AddressType.V6;
        _storage = address;
    }

    public AddressV4 AsV4() => new(_storage.DataU32[0]);
    public AddressV6 AsV6() => _storage;

    public readonly override string? ToString()
    {
        return Type switch
        {
            AddressType.V4 => AsV4().ToString(),
            AddressType.V6 => AsV6().ToString(),
            _ => null
        };
    }

    public static implicit operator Address(AddressV4 address) => new(address);
    public static implicit operator Address(AddressV6 address) => new(address);
}
