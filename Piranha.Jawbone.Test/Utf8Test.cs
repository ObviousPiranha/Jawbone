using System;
using System.Text;
using Xunit;

namespace Piranha.Jawbone.Test;

public class Utf8Test
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(0x555)]
    [InlineData(0x5555)]
    [InlineData(0x100000)]
    public void Utf8RoundTrip(int utf32)
    {
        var s = char.ConvertFromUtf32(utf32);
        var encoded = Encoding.UTF8.GetBytes(s);
        var bytes = new byte[8];
        var length = Utf8.Encode(bytes, utf32);
        Assert.True(bytes.AsSpan(0, length).SequenceEqual(encoded));

        var decoded = char.ConvertToUtf32(s, 0);
        var (codePoint, decodeLength) = Utf8.ReadCodePoint(bytes.AsSpan(0, length));
        Assert.Equal(length, decodeLength);
        Assert.Equal(decoded, codePoint);
    }
}
