using System;
using System.Linq;

namespace Jawbone.Test;

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

    [Fact]
    public void PushBack_MultipleItems_IncreasesCount()
    {
        ReadOnlySpan<int> items = [1, 2, 3, 4, 5, 6];
        var list = new LoopyList<int>();
        list.PushBack(items);
        Assert.Equal(items.Length, list.Count);

        for (int i = 0; i < items.Length; ++i)
            Assert.Equal(items[i], list[i]);
    }

    [Fact]
    public void PushFront_MultipleItems_IncreasesCount()
    {
        ReadOnlySpan<int> items = [1, 2, 3, 4, 5, 6];
        var list = new LoopyList<int>();
        list.PushAllFront(items);
        Assert.Equal(items.Length, list.Count);

        for (int i = 0; i < items.Length; ++i)
            Assert.Equal(items[i], list[i]);
    }

    [Fact]
    public void AsEnumerable_YieldsCorrectValues()
    {
        var list = new LoopyList<int>();
        ReadOnlySpan<int> items = [1, 2, 3, 4, 5, 6];
        list.PushBack(items);
        var array = list.AsEnumerable().ToArray();
        Assert.Equal(items, array);
    }

    [Fact]
    public void PopFrontWhile_RemovesValues()
    {
        var list = new LoopyList<int>();
        list.PushAllFront(0, 0, 0, 0);
        list.PushBack(1, 1, 1, 1);
        Assert.Equal(8, list.Count);
        list.PopFrontWhile(static n => n == 0);
        Assert.Equal(4, list.Count);
        Assert.True(list.AsEnumerable().All(static n => n == 1));
    }

    [Fact]
    public void PopBackWhile_RemovesValues()
    {
        var list = new LoopyList<int>();
        list.PushAllFront(0, 0, 0, 0);
        list.PushBack(1, 1, 1, 1);
        Assert.Equal(8, list.Count);
        list.PopBackWhile(static n => n == 1);
        Assert.Equal(4, list.Count);
        Assert.True(list.AsEnumerable().All(static n => n == 0));
    }

    [Fact]
    public void SequenceEqual_ContiguousList()
    {
        var list = new LoopyList<int>();
        ReadOnlySpan<int> items = [1, 2, 3, 4, 5, 6, 7, 8];
        list.PushBack(items);
        Assert.True(list.IsContiguous);
        Assert.True(list.AsSpan().SequenceEqual(items));
    }

    [Fact]
    public void SequenceEqual_NonContiguousList()
    {
        var list = new LoopyList<int>();
        ReadOnlySpan<int> items = [1, 2, 3, 4, 5, 6, 7, 8];
        list.PushBack(items[4..]);
        list.PushAllFront(items[..4]);
        Assert.False(list.IsContiguous);
        Assert.True(list.AsSpan().SequenceEqual(items));
    }

    [Fact]
    public void AsSpan_FullList()
    {
        var list = new LoopyList<int>();
        list.PushBack(-1);
        while (list.Count < list.Capacity)
            list.PushBack(1);
        // Force wrap-around.
        list.RemoveFront(1);
        list.PushBack(2);
        Assert.Equal(list.Capacity, list.Count);
        var spans = list.AsSpan();
        Assert.Equal(list.Capacity, spans.Length);
    }
}
