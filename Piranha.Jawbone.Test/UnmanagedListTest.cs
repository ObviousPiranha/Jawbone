using System;
using System.Text;
using Xunit;

namespace Piranha.Jawbone.Test;

public class UnmanagedListTest
{
    [Fact]
    public void PushAndPop()
    {
        var list = new UnmanagedList<int>();
        Assert.True(list.IsEmpty);

        list.AppendMany(1, 2, 3, 4);
        Assert.Equal(4, list.Count);

        list.Pop();
        Assert.Equal(3, list.Count);
        Assert.True(list.AsSpan().SequenceEqual(new int[] { 1, 2, 3 }));

        list.Clear();
        Assert.True(list.IsEmpty);
        Assert.Throws<IndexOutOfRangeException>(() => list.Pop());
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdf")]
    [InlineData("surrogate pair \U0001F01C")]
    public void AppendUtf32_String(string expected)
    {
        var list = new UnmanagedList<int>();
        list.AppendUtf32(expected);
        var actual = Encoding.UTF32.GetString(list.Bytes);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdf")]
    [InlineData("surrogate pair \U0001F01C")]
    public void AppendUtf32_StringBuilder(string expected)
    {
        var stringBuilder = new StringBuilder(expected);
        var list = new UnmanagedList<int>();
        list.AppendUtf32(stringBuilder);
        var actual = Encoding.UTF32.GetString(list.Bytes);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void AppendUtf32_Int32(int value)
    {
        var list = new UnmanagedList<int>();
        list.AppendUtf32(value);
        var actual = Encoding.UTF32.GetString(list.Bytes);
        var expected = value.ToString();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(uint.MaxValue)]
    public void AppendUtf32_UInt32(uint value)
    {
        var list = new UnmanagedList<int>();
        list.AppendUtf32(value);
        var actual = Encoding.UTF32.GetString(list.Bytes);
        var expected = value.ToString();
        Assert.Equal(expected, actual);
    }
}
