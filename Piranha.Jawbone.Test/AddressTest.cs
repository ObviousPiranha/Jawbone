using Piranha.Jawbone.Net;
using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Piranha.Jawbone.Test;

public class AddressTest
{
    [Fact]
    public void AddressInvariants()
    {
        Assert.Equal(4, Unsafe.SizeOf<Address32>());
        Assert.Equal(16, Unsafe.SizeOf<Address128>());

        Assert.False(Address32.Any.IsLoopback);
        Assert.False(Address128.Any.IsLoopback);
        Assert.True(Address32.Local.IsLoopback);
        Assert.True(Address128.Local.IsLoopback);

        Assert.Throws<ArgumentNullException>(() => Address32.Parse(null!, null));
        Assert.Throws<ArgumentNullException>(() => Address128.Parse(null!, null));
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

    [Theory]
    [MemberData(nameof(NotLinkLocal128))]
    public void Address128_IsNotLinkLocal(Address128 address)
    {
        Assert.False(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void Address32_RoundTripParseString(Address32 expected)
    {
        var asString = expected.ToString();
        var actual = Address32.Parse(asString, null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void Address32_RoundTripParseSpan(Address32 expected)
    {
        var asString = expected.ToString();
        var actual = Address32.Parse(asString.AsSpan(), null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void Address32_RoundTripTryParseString(Address32 expected)
    {
        var asString = expected.ToString();
        Assert.True(Address32.TryParse(asString, null, out var actual));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void Address32_RoundTripTryParseSpan(Address32 expected)
    {
        var asString = expected.ToString();
        Assert.True(Address32.TryParse(asString.AsSpan(), null, out var actual));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdf")]
    [InlineData("127.0.0.")]
    [InlineData("1,1,1,1")]
    [InlineData("253.254.255.256")]
    public void Address32_FailsParsing(string s)
    {
        Assert.False(Address32.TryParse(s, null, out var result));
        Assert.True(result.IsDefault);
        Assert.False(Address32.TryParse(s.AsSpan(), null, out result));
        Assert.True(result.IsDefault);
        Assert.Throws<FormatException>(() => Address32.Parse(s, null));
        Assert.Throws<FormatException>(() => Address32.Parse(s.AsSpan(), null));
    }

    public static TheoryData<Address32> LinkLocal32 => new()
    {
        new(169, 254, 0, 0),
        new(169, 254, 127, 127),
        new(169, 254, 255, 255)
    };

    public static TheoryData<Address32> NotLinkLocal32 => new()
    {
        Address32.Any,
        Address32.Local,
        Address32.Broadcast,
        new(169, 200, 0, 0),
        new(170, 254, 0, 0)
    };

    public static TheoryData<Address128> LinkLocal128 => new()
    {
        Address128.Create(static span => MakeLinkLocal(span))
    };

    public static TheoryData<Address128> NotLinkLocal128 => new()
    {
        Address128.Any,
        Address128.Local,
        Address128.Create(static span => span.Fill(0xab))
    };

    public static TheoryData<Address32> RoundTripParse32 => new()
    {
        Address32.Any,
        Address32.Local,
        Address32.Broadcast,
        new(192, 168, 0, 1),
        new(0, 1, 2, 3)
    };

    private static void MakeLinkLocal(Span<byte> bytes)
    {
        bytes[0] = 0xfe;
        bytes[1] = 0x80;
    }
}
