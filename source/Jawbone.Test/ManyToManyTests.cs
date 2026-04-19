using System;
using System.Numerics;

namespace Jawbone.Test;

public class ManyToManyTests
{
    [Fact]
    public void Test()
    {
        var m2m = new ManyToMany<int, string>();
        var expectedCount = 10;

        for (int i = 1; i <= expectedCount; ++i)
        {
            Assert.True(m2m.TryAdd(0, i.ToString()));
            Assert.True(m2m.TryAdd(i, "0"));
        }

        Assert.Equal(expectedCount, m2m.GetLeftValues("0").Length);
        Assert.Equal(expectedCount, m2m.GetRightValues(0).Length);

        for (int i = 1; i <= expectedCount; ++i)
        {
            Assert.Equal(1, m2m.GetLeftValues(i.ToString()).Length);
            Assert.Equal(1, m2m.GetRightValues(i).Length);
        }
    }

    [Fact]
    public void TryAdd_ExistingPair_ReturnsFalse()
    {
        TestDoubleAdd("Hello", 1);
        TestDoubleAdd(555m, new DateTime(2000, 1, 1));
        TestDoubleAdd(BigInteger.One, 55f);
    }

    private static void TestDoubleAdd<T0, T1>(T0 left, T1 right) where T0 : notnull where T1 : notnull
    {
        var manyToMany = new ManyToMany<T0, T1>();
        Assert.True(manyToMany.TryAdd(left, right));
        Assert.False(manyToMany.TryAdd(left, right));
    }
}
