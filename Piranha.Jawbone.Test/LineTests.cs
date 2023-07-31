using Xunit;

namespace Piranha.Jawbone.Test;

public class LineTests
{
    [Theory]
    [InlineData(-5, -5, 5, 5, -7, 7, 7, -7, 0, 0)]
    [InlineData(1, 1, 3, 3, 1, 3, 3, 1, 2, 2)]
    [InlineData(1, 1, 6, 1, -5, -5, -5, -4, -5, 1)]
    public void Intersection(
        int x1,
        int y1,
        int x2,
        int y2,
        int x3,
        int y3,
        int x4,
        int y4,
        int expectedX,
        int expectedY)
    {
        var success = Line.Intersects(
            x1,
            y1,
            x2,
            y2,
            x3,
            y3,
            x4,
            y4,
            out var actualX,
            out var actualY);
        Assert.True(success);
        Assert.Equal(expectedX, actualX);
        Assert.Equal(expectedY, actualY);
    }

    [Theory]
    [InlineData(-5, 1, -2, 1, 1, 1)]
    [InlineData(0, -1, 1, 0, 2, 1)]
    public void IntersectionAtX(
        int x1,
        int y1,
        int x2,
        int y2,
        int x,
        int expectedY)
    {
        var success = Line.IntersectsAtX(
            x1,
            y1,
            x2,
            y2,
            x,
            out var actualY);
        Assert.True(success);
        Assert.Equal(expectedY, actualY);
    }

    [Theory]
    [InlineData(-2, -4, -1, -2, 0, 0)]
    [InlineData(0, -3, -1, -1, 1, -2)]
    public void IntersectionAtY(
        int x1,
        int y1,
        int x2,
        int y2,
        int y,
        int expectedX)
    {
        var success = Line.IntersectsAtY(
            x1,
            y1,
            x2,
            y2,
            y,
            out var actualX);
        Assert.True(success);
        Assert.Equal(expectedX, actualX);
    }
}
