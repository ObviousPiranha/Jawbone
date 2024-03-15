using System;
using System.Numerics;
using Xunit;

namespace Piranha.Jawbone.Test;

public class AnchorMathTests
{
    [Theory]
    [MemberData(nameof(VectorData))]
    public void AlignTwoLineSegments(
        Vector2 a,
        Vector2 b,
        Vector2 c,
        Vector2 d)
    {
        var matrix = AnchorMath.Align(a, b, c, d);
        var aa = Vector2.Transform(a, matrix);
        var bb = Vector2.Transform(b, matrix);
        Assert.Equal(c.X, aa.X, 4);
        Assert.Equal(c.Y, aa.Y, 4);
        Assert.Equal(d.X, bb.X, 4);
        Assert.Equal(d.Y, bb.Y, 4);
    }

    public static TheoryData<Vector2, Vector2, Vector2, Vector2> VectorData => new()
    {
        { new(1f, 1f), new(2f, 4f), new(-3f, -3f), new(-6f, -6f) },
        { new(0f, 0f), new(1f, MathF.PI), new(-3f, -3f), new(-6f, -6f) }
    };
}