using System;
using System.Text;
using Xunit;

namespace Jawbone.Test;

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
    // https://www.w3.org/2001/06/utf-8-test/UTF-8-demo.html
    [InlineData("∮ E⋅da = Q,  n → ∞, ∑ f(i) = ∏ g(i), ∀x∈ℝ: ⌈x⌉ = −⌊−x⌋, α ∧ ¬β = ¬(¬α ∨ β)")]
    [InlineData("Σὲ γνωρίζω ἀπὸ τὴν κόψη")]
    public void AppendUtf8_Utf32(string utf16)
    {
        var utf8 = Encoding.UTF8.GetBytes(utf16);
        var utf32 = Encoding.UTF32.GetBytes(utf16);
        var listUtf32 = new UnmanagedList<int>().AppendUtf32(utf16);
        var listUtf8 = new UnmanagedList<byte>().AppendUtf8(listUtf32.AsSpan());

        Assert.True(listUtf32.Bytes.SequenceEqual(utf32));
        Assert.True(listUtf8.AsSpan().SequenceEqual(utf8));
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
