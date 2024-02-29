using System;
using System.Buffers;
using System.Runtime.InteropServices;
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

    [Theory]
    [InlineData("")]
    [InlineData("asdf")]
    [InlineData("surrogate pair \U0001F01C")]
    // https://www.w3.org/2001/06/utf-8-test/UTF-8-demo.html
    [InlineData("∮ E⋅da = Q,  n → ∞, ∑ f(i) = ∏ g(i), ∀x∈ℝ: ⌈x⌉ = −⌊−x⌋, α ∧ ¬β = ¬(¬α ∨ β)")]
    [InlineData("Σὲ γνωρίζω ἀπὸ τὴν κόψη")]
    public void Utf8Enumeration(string text)
    {
        var utf8 = (ReadOnlyUtf8Span)Encoding.UTF8.GetBytes(text);
        var bytes = Encoding.UTF32.GetBytes(text);
        var utf32 = MemoryMarshal.Cast<byte, int>(bytes);
        var index = 0;

        foreach (var codePoint in utf8)
            Assert.Equal(utf32[index++], codePoint);
    }
}
