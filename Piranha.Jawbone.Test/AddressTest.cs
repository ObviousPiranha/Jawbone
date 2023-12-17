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
        Assert.Equal(4, Unsafe.SizeOf<AddressV4>());
        Assert.Equal(20, Unsafe.SizeOf<AddressV6>());

        Assert.True(AddressV4.Local.IsLoopback);
        Assert.True(AddressV6.Local.IsLoopback);

        Assert.Throws<ArgumentNullException>(() => AddressV4.Parse(null!, null));
        Assert.Throws<ArgumentNullException>(() => AddressV6.Parse(null!, null));
    }

    [Theory]
    [MemberData(nameof(LinkLocal32))]
    public void AddressV4_IsLinkLocal(AddressV4 address)
    {
        Assert.True(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(NotLinkLocal32))]
    public void AddressV4_IsNotLinkLocal(AddressV4 address)
    {
        Assert.False(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(LinkLocal128))]
    public void AddressV6_IsLinkLocal(AddressV6 address)
    {
        Assert.True(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(NotLinkLocal128))]
    public void AddressV6_IsNotLinkLocal(AddressV6 address)
    {
        Assert.False(address.IsLinkLocal);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void AddressV4_RoundTripParseString(AddressV4 expected)
    {
        var asString = expected.ToString();
        var actual = AddressV4.Parse(asString, null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void AddressV4_RoundTripParseSpan(AddressV4 expected)
    {
        var asString = expected.ToString();
        var actual = AddressV4.Parse(asString.AsSpan(), null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void AddressV4_RoundTripTryParseString(AddressV4 expected)
    {
        var asString = expected.ToString();
        Assert.True(AddressV4.TryParse(asString, null, out var actual));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse32))]
    public void AddressV4_RoundTripTryParseSpan(AddressV4 expected)
    {
        var asString = expected.ToString();
        Assert.True(AddressV4.TryParse(asString.AsSpan(), null, out var actual));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdf")]
    [InlineData("127.0.0.")]
    [InlineData("1,1,1,1")]
    [InlineData("253.254.255.256")]
    public void AddressV4_FailsParsing(string s)
    {
        Assert.False(AddressV4.TryParse(s, null, out var result));
        Assert.True(result.IsDefault);
        Assert.False(AddressV4.TryParse(s.AsSpan(), null, out result));
        Assert.True(result.IsDefault);
        Assert.Throws<FormatException>(() => AddressV4.Parse(s, null));
        Assert.Throws<FormatException>(() => AddressV4.Parse(s.AsSpan(), null));
    }

    [Theory]
    [MemberData(nameof(RoundTripParse128))]
    public void AddressV6_RoundTripParseString(AddressV6 expected)
    {
        var asString = expected.ToString();
        var actual = AddressV6.Parse(asString, null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse128))]
    public void AddressV6_RoundTripParseSpan(AddressV6 expected)
    {
        var asString = expected.ToString();
        var actual = AddressV6.Parse(asString.AsSpan(), null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse128))]
    public void AddressV6_RoundTripTryParseString(AddressV6 expected)
    {
        var asString = expected.ToString();
        Assert.True(AddressV6.TryParse(asString, null, out var actual));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(RoundTripParse128))]
    public void AddressV6_RoundTripTryParseSpan(AddressV6 expected)
    {
        var asString = expected.ToString();
        Assert.True(AddressV6.TryParse(asString.AsSpan(), null, out var actual));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData(":::")]
    [InlineData("c3ee:abce:12345::2")]
    [InlineData("cf:0::71ef:91e::6")]
    public void AddressV6_FailsParsing(string s)
    {
        Assert.False(AddressV6.TryParse(s, null, out var result));
        Assert.True(result.IsDefault);
        Assert.False(AddressV6.TryParse(s.AsSpan(), null, out result));
        Assert.True(result.IsDefault);
        Assert.Throws<FormatException>(() => AddressV6.Parse(s, null));
        Assert.Throws<FormatException>(() => AddressV6.Parse(s.AsSpan(), null));
    }

    public static TheoryData<AddressV4> LinkLocal32 => new()
    {
        new(169, 254, 0, 0),
        new(169, 254, 127, 127),
        new(169, 254, 255, 255)
    };

    public static TheoryData<AddressV4> NotLinkLocal32 => new()
    {
        AddressV4.Any,
        AddressV4.Local,
        AddressV4.Broadcast,
        new(169, 200, 0, 0),
        new(170, 254, 0, 0)
    };

    public static TheoryData<AddressV6> LinkLocal128 => new()
    {
        AddressV6.Create(static span => MakeLinkLocal(span))
    };

    public static TheoryData<AddressV6> NotLinkLocal128 => new()
    {
        AddressV6.Local,
        AddressV6.Create(static span => span.Fill(0xab))
    };

    public static TheoryData<AddressV4> RoundTripParse32 => new()
    {
        AddressV4.Any,
        AddressV4.Local,
        AddressV4.Broadcast,
        new(192, 168, 0, 1),
        new(0, 1, 2, 3)
    };

    public static TheoryData<AddressV6> RoundTripParse128 => new()
    {
        AddressV6.Local,
        AddressV6.Create(static span => span.Fill(0xab)),
        AddressV6.Create(static span =>
        {
            span[3] = 0xb;
            span[11] = 0xce;
        }),
        AddressV4.Local.MapToV6(),
        AddressV4.Broadcast.MapToV6()
    };

    private static void MakeLinkLocal(Span<byte> bytes)
    {
        bytes[0] = 0xfe;
        bytes[1] = 0x80;
    }
}
