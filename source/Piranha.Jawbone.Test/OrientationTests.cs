using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Piranha.Jawbone.Test;

public class OrientationTests
{
    public static IEnumerable<Orientation> AllOrientations()
    {
        for (int i = 0; i < 8; ++i)
            yield return (Orientation)i;
    }

    public static IEnumerable<object[]> AllOrientationsAsInputs()
    {
        return AllOrientations().Select(
            static x => new object[] { x });
    }

    [Fact]
    public void OrientationInvariants()
    {
        Assert.Equal(0, (int)Orientation.None);
        Assert.Equal(1, (int)Orientation.R1);
        Assert.Equal(2, (int)Orientation.R2);
        Assert.Equal(3, (int)Orientation.R3);
        Assert.Equal(4, (int)Orientation.F);
        Assert.Equal(5, (int)Orientation.FR1);
        Assert.Equal(6, (int)Orientation.FR2);
        Assert.Equal(7, (int)Orientation.FR3);
    }

    [Theory]
    [MemberData(nameof(AllOrientationsAsInputs))]
    public void OrientationsComeFullCircle(Orientation o)
    {
        Assert.NotEqual(o, o.Rotate1());
        Assert.NotEqual(o, o.Rotate2());
        Assert.NotEqual(o, o.Rotate3());
        Assert.NotEqual(o, o.FlipH());
        Assert.NotEqual(o, o.FlipV());

        Assert.Equal(o, o.Rotate1().Rotate1().Rotate1().Rotate1());
        Assert.Equal(o, o.Rotate1().Rotate3());
        Assert.Equal(o, o.Rotate3().Rotate1());
        Assert.Equal(o, o.Rotate2().Rotate2());
        Assert.Equal(o, o.Rotate3().Rotate3().Rotate3().Rotate3());
        Assert.Equal(o, o.FlipH().FlipH());
        Assert.Equal(o, o.FlipV().FlipV());
        Assert.Equal(o, o.FlipH().Rotate1().Rotate1().Rotate1().Rotate1().FlipH());
        Assert.Equal(o, o.FlipV().Rotate3().Rotate3().Rotate3().Rotate3().FlipV());
        Assert.Equal(o, o.FlipH().Rotate1().FlipH().Rotate1());
        Assert.Equal(o, o.FlipV().Rotate1().FlipV().Rotate1());
        Assert.Equal(o, o.FlipH().Rotate2().FlipV());
    }
}
