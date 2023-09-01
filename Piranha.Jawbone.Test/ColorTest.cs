using Xunit;

namespace Piranha.Jawbone.Test;

public class ColorTest
{
    public static TheoryData<ColorRgb24> ColorsRgb24 => new()
    {
        new ColorRgb24(),
        new ColorRgb24(1, 1, 1),
        new ColorRgb24(7, 99, 255),
        new ColorRgb24(255, 255, 255),
    };

    [Theory]
    [MemberData(nameof(ColorsRgb24))]
    public void ParseUtf16_RoundTrip(ColorRgb24 expected)
    {
        var asString = expected.ToString();
        var actual = ColorRgb24.Parse(asString, null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ColorsRgb24))]
    public void ParseUtf8_RoundTrip(ColorRgb24 expected)
    {
        var utf8 = new byte[8];
        expected.WriteUtf8(utf8);
        var actual = ColorRgb24.ParseUtf8(utf8);
        Assert.Equal(expected, actual);
    }
}
