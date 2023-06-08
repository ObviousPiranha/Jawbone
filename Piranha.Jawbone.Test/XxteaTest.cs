using System;
using System.Collections.Immutable;
using Xunit;

namespace Piranha.Jawbone.Test;

public class XxteaTest
{
    private static readonly ImmutableArray<uint> s_key = ImmutableArray.Create<uint>(
        0xdeadbeef,
        0xeeeeeeee,
        0x1,
        0xffffffff);

    private static ReadOnlySpan<uint> Key => s_key.AsSpan();

    [Fact]
    public void ThrowsOnInvalidInput()
    {
        Assert.Throws<ArgumentException>(() => Xxtea.Encrypt(default, Key));

        var v = new uint[] { 1U, 2U, 3U, 4U };
        Assert.Throws<ArgumentException>(() => Xxtea.Encrypt(v, default));
    }

    [Fact]
    public void RoundTrip()
    {
        var expected = UnmanagedList<uint>.CreateArray(64, static span => Random.Shared.NextBytes(span));
        var actual = expected.AsSpan().ToArray();
        Xxtea.Encrypt(actual, Key);
        Assert.False(expected.AsSpan().SequenceEqual(actual));
        Xxtea.Decrypt(actual, Key);
        Assert.True(expected.AsSpan().SequenceEqual(actual));
    }

    [Fact]
    public void RoundTripBytes()
    {
        // Purposely avoid multiple of 4.
        var expected = new byte[61];
        Random.Shared.NextBytes(expected);
        var encrypted = new byte[expected.Length + 8];
        int encryptedLength = Xxtea.Encrypt(expected, encrypted, Key);
        Assert.False(expected.AsSpan().SequenceEqual(encrypted.AsSpan(0, encryptedLength)));
        var actual = new byte[encryptedLength];
        int decryptedLength = Xxtea.Decrypt(encrypted.AsSpan(0, encryptedLength), actual, Key);
        Assert.Equal(encryptedLength, decryptedLength);
        Assert.True(expected.AsSpan().SequenceEqual(actual.AsSpan(0, expected.Length)));

        foreach (var item in actual.AsSpan(expected.Length))
            Assert.Equal(0, item);
    }

    [Fact]
    public void ExtraLongRoundTrip()
    {
        const int TripCount = 8;
        var expected = UnmanagedList<uint>.CreateArray(64, static span => Random.Shared.NextBytes(span));
        var actual = expected.AsSpan().ToArray();
        for (int i = 0; i < TripCount; ++i)
            Xxtea.Encrypt(actual, Key);
        Assert.False(expected.AsSpan().SequenceEqual(actual));
        for (int i = 0; i < TripCount; ++i)
            Xxtea.Decrypt(actual, Key);
        Assert.True(expected.AsSpan().SequenceEqual(actual));
    }
}
