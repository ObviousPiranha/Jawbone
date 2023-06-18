using Piranha.Jawbone.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace Piranha.Jawbone.Test;

public class AddressTest
{
    [Fact]
    public void AddressesAreExactSizes()
    {
        Assert.Equal(4, Unsafe.SizeOf<Address32>());
        Assert.Equal(16, Unsafe.SizeOf<Address128>());
    }

    [Theory]
    [MemberData(nameof(LinkLocal32))]
    public void Address32_IsLinkLocal(Address32 address)
    {
        Assert.True(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(NotLinkLocal32))]
    public void Address32_IsNotLinkLocal(Address32 address)
    {
        Assert.False(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(LinkLocal128))]
    public void Address128_IsLinkLocal(Address128 address)
    {
        Assert.True(address.IsLinkLocal);
    }

    public static TheoryData<Address32> LinkLocal32 => new()
    {
        { new(169, 254, 0, 0) },
        { new(169, 254, 127, 127) },
        { new(169, 254, 255, 255) }
    };

    public static TheoryData<Address32> NotLinkLocal32 => new()
    {
        { default },
        { Address32.Local },
        { Address32.Broadcast },
        { new(169, 200, 0, 0) },
        { new(170, 254, 0, 0) }
    };

    public static TheoryData<Address128> LinkLocal128 => new()
    {
        { Address128.Create(static span => MakeLinkLocal(span))}
    };

    private static void MakeLinkLocal(Span<byte> bytes)
    {
        bytes[0] = 0xfe;
        bytes[1] = 0x80;
    }
}