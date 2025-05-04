using System;
using System.Numerics;

namespace Piranha.Jawbone.Test;

public class AnchorMathTests
{
    private static void AssertEqual(
        Vector2 a,
        Vector2 b,
        int precision)
    {
        Assert.Equal(a.X, b.X, precision);
        Assert.Equal(a.Y, b.Y, precision);
    }

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
        AssertEqual(c, aa, 4);
        AssertEqual(d, bb, 4);
    }

    public static TheoryData<Vector2, Vector2, Vector2, Vector2> VectorData => new()
    {
        { new(1f, 1f), new(2f, 4f), new(-3f, -3f), new(-6f, -6f) },
        { new(0f, 0f), new(1f, MathF.PI), new(-3f, -3f), new(-6f, -6f) },
        { new(0.5f, 0.75f), new(9f, MathF.PI), new(-2f, -MathF.PI), new(-7f, -1f) },
    };
}
