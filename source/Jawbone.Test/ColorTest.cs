
namespace Jawbone.Test;

public class ColorTest
{
    public static TheoryData<ColorRgb24> ColorsRgb24 => new()
    {
        new ColorRgb24(),
        new ColorRgb24(1, 1, 1),
        new ColorRgb24(7, 99, 255),
        new ColorRgb24(255, 255, 255),
    };

    public static TheoryData<ColorRgba32> ColorsRgba32 => new()
    {
        new ColorRgba32(),
        new ColorRgba32(1, 1, 1, 1),
        new ColorRgba32(7, 99, 255, 255),
        new ColorRgba32(255, 255, 255, 255),
    };

    [Theory]
    [MemberData(nameof(ColorsRgb24))]
    public void ColorRgb24_ParseUtf16_RoundTrip(ColorRgb24 expected)
    {
        var asString = expected.ToString();
        var actual = ColorRgb24.Parse(asString, null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ColorsRgb24))]
    public void ColorRgb24_ParseUtf8_RoundTrip(ColorRgb24 expected)
    {
        var utf8 = new byte[8];
        expected.WriteUtf8(utf8);
        var actual = ColorRgb24.ParseUtf8(utf8);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ColorsRgba32))]
    public void ColorRgba32_ParseUtf16_RoundTrip(ColorRgba32 expected)
    {
        var asString = expected.ToString();
        var actual = ColorRgba32.Parse(asString, null);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ColorsRgba32))]
    public void ColorRgba32_ParseUtf8_RoundTrip(ColorRgba32 expected)
    {
        var utf8 = new byte[10];
        expected.WriteUtf8(utf8);
        var actual = ColorRgba32.ParseUtf8(utf8);
        Assert.Equal(expected, actual);
    }
}
