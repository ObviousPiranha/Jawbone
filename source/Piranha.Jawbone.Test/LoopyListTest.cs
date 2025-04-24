using System;

namespace Piranha.Jawbone.Test;

public class LoopyListTest
{
    [Fact]
    public void NewInstance_IsEmpty()
    {
        var list = new LoopyList<int>();
        Assert.Equal(0, list.Count);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[0]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
    }

    [Fact]
    public void PushBack_IncreasesCount()
    {
        var list = new LoopyList<int>();
        var count = 100;
        for (int i = 0; i < count; ++i)
        {
            Assert.Equal(i, list.Count);
            list.PushBack(i);
            Assert.Equal(i, list[i]);
        }

        Assert.Equal(count, list.Count);
    }

    [Fact]
    public void PushFront_IncreasesCount()
    {
        var list = new LoopyList<int>();
        var count = 100;
        for (int i = 0; i < count; ++i)
        {
            Assert.Equal(i, list.Count);
            list.PushFront(i);
            Assert.Equal(i, list[0]);
        }

        Assert.Equal(count, list.Count);
    }

    [Fact]
    public void Clear_ReducesCountToZero()
    {
        var list = new LoopyList<int>();
        var count = 50;

        for (int i = 0; i < count; ++i)
            list.PushBack(i);

        Assert.Equal(count, list.Count);
        list.Clear();
        Assert.Equal(0, list.Count);
    }
}
