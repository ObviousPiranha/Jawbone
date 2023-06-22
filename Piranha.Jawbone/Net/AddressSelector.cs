using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net;

public readonly struct AddressSelector<TAddress> : IEquatable<AddressSelector<TAddress>> where TAddress : unmanaged, IAddress<TAddress>
{
    public readonly TAddress Address { get; init; }

    public bool Equals(AddressSelector<TAddress> other) => Address.Equals(other.Address);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AddressSelector<TAddress> other && Equals(other);
    public override int GetHashCode() => Address.GetHashCode();
    public override string? ToString() => Address.ToString();

    public static implicit operator AddressSelector<TAddress>(TAddress address) => new() { Address = address };
    public static implicit operator TAddress(AddressSelector<TAddress> addressProvider) => addressProvider.Address;

    public static implicit operator AddressSelector<TAddress>(AnyAddress _) => new() { Address = TAddress.Any };
    public static implicit operator AddressSelector<TAddress>(LocalAddress _) => new() { Address = TAddress.Local };

    public static bool operator ==(AddressSelector<TAddress> a, AddressSelector<TAddress> b) => a.Equals(b);
    public static bool operator !=(AddressSelector<TAddress> a, AddressSelector<TAddress> b) => !a.Equals(b);
}
