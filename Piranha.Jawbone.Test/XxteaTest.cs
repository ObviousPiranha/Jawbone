using Piranha.Jawbone.Collections;
using Piranha.Jawbone.Tools;
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
    public void RoundTrip()
    {
        var expected = new UnmanagedList<uint>();
        for (int i = 0; i < 16; ++i)
            expected.Add((uint)Random.Shared.Next());
        var actual = new UnmanagedList<uint>();
        actual.AddAll(expected.Items);
        Xxtea.Encrypt(actual.Items, Key);
        Assert.False(expected.Items.SequenceEqual(actual.Items));
        Xxtea.Decrypt(actual.Items, Key);
        Assert.True(expected.Items.SequenceEqual(actual.Items));
    }
}
