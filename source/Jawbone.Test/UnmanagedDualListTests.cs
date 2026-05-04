using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Jawbone.Test;

public class UnmanagedDualListTests
{
    [Fact]
    public void AddSingleItems()
    {
        var expectedCount = 256;
        var expectedLeft = new short[expectedCount];
        var expectedRight = new long[expectedCount];

        for (int i = 0; i < expectedCount; ++i)
        {
            expectedLeft[i] = (short)(100 + i);
            expectedRight[i] = 2000 + i;
        }

        var list = new UnmanagedDualList<short, long>();

        for (int i = 0; i < expectedCount; ++i)
            list.Add(expectedLeft[i], expectedRight[i]);
        
        Assert.Equal(expectedCount, list.Count);
        Assert.Equal(expectedLeft, list.Left);
        Assert.Equal(expectedRight, list.Right);

        DoFinalAssertions(list);
    }

    [Fact]
    public void AddSpans()
    {
        var expectedCount = 256;
        var expectedLeft = new short[expectedCount];
        var expectedRight = new long[expectedCount];

        for (int i = 0; i < expectedCount; ++i)
        {
            expectedLeft[i] = (short)(100 + i);
            expectedRight[i] = 2000 + i;
        }

        var list = new UnmanagedDualList<short, long>();
        list.AddSpans(expectedLeft, expectedRight);

        Assert.Equal(expectedCount, list.Count);
        Assert.Equal(expectedLeft, list.Left);
        Assert.Equal(expectedRight, list.Right);

        DoFinalAssertions(list);
    }

    [Fact]
    public void AddUnevenSpans()
    {
        var left = new short[25];
        var right = new long[left.Length + 1];

        var list = new UnmanagedDualList<short, long>();
        Assert.Throws<ArgumentException>(() => list.AddSpans(left, right));
    }

    [Fact]
    public void AddEnumerables()
    {
        var expectedCount = 256;
        var expectedLeft = new short[expectedCount];
        var expectedRight = new long[expectedCount];

        for (int i = 0; i < expectedCount; ++i)
        {
            expectedLeft[i] = (short)(100 + i);
            expectedRight[i] = 2000 + i;
        }

        var list = new UnmanagedDualList<short, long>();
        list.AddEnumerables(Wrap(expectedLeft), Wrap(expectedRight));

        Assert.Equal(expectedCount, list.Count);
        Assert.Equal(expectedLeft, list.Left);
        Assert.Equal(expectedRight, list.Right);

        DoFinalAssertions(list);
    }

    [Fact]
    public void AddUnevenEnumerables()
    {
        var left = new short[25];
        var right = new long[left.Length + 1];

        var list = new UnmanagedDualList<short, long>();
        Assert.Throws<ArgumentException>(() => list.AddEnumerables(Wrap(left), Wrap(right)));
    }

    [Fact]
    public void ResizeClearsValues()
    {
        var expectedValue = 99;
        var expectedValues = new int[16];
        expectedValues.AsSpan().Fill(expectedValue);

        var list = new UnmanagedDualList<int, int>();
        list.AddSpans(expectedValues, expectedValues);

        Assert.Equal(expectedValues, list.Left);
        Assert.Equal(expectedValues, list.Right);

        var expectedCount = expectedValues.Length / 2;
        list.Count = expectedCount;
        Assert.Equal(expectedCount, list.Count);

        var zeroes = new int[expectedCount];
        list.Count = expectedValues.Length;
        Assert.Equal(expectedValues.AsSpan(0, expectedCount), list.Left[..expectedCount]);
        Assert.Equal(zeroes, list.Left[expectedCount..]);
        Assert.Equal(expectedValues.AsSpan(0, expectedCount), list.Right[..expectedCount]);
        Assert.Equal(zeroes, list.Right[expectedCount..]);

        DoFinalAssertions(list);
    }

    private static void DoFinalAssertions<TLeft, TRight>(UnmanagedDualList<TLeft, TRight> list)
        where TLeft : unmanaged
        where TRight : unmanaged
    {
        var left = list.Left;
        var right = list.Right;

        Assert.Equal(list.Count, left.Length);
        Assert.Equal(list.Count, right.Length);

        for (int i = 0; i < list.Count; ++i)
        {
            {
                var pair = list[i];
                Assert.Equal(pair.Left, left[i]);
                Assert.Equal(pair.Right, right[i]);
            }

            {
                Index index = i;
                var pair = list[index];
                Assert.Equal(pair.Left, left[i]);
                Assert.Equal(pair.Right, right[i]);
            }
        }

        var leftBytes = MemoryMarshal.AsBytes(left);
        Assert.Equal(leftBytes, list.LeftBytes);

        var rightBytes = MemoryMarshal.AsBytes(right);
        Assert.Equal(rightBytes, list.RightBytes);

        list.Clear();
        Assert.Equal(0, list.Count);
        Assert.Equal(0, list.Left.Length);
        Assert.Equal(0, list.LeftBytes.Length);
        Assert.Equal(0, list.Right.Length);
        Assert.Equal(0, list.RightBytes.Length);
    }

    private static IEnumerable<T> Wrap<T>(IEnumerable<T> enumerable)
    {
        foreach (var item in enumerable)
            yield return item;
    }
}