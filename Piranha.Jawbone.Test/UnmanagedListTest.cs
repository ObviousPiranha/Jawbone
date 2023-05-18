using Piranha.Jawbone.Collections;
using System.Text;
using Xunit;

namespace Piranha.Jawbone.Test;

public class UnmanagedListTest
{
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
