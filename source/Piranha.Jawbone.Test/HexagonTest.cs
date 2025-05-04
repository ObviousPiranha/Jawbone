using Piranha.Jawbone.Hexagon;

namespace Piranha.Jawbone.Test;

public class HexagonTest
{
    [Fact]
    public void RotateHexagons()
    {
        var axial = new Axial32(11, 17);
        Assert.Equal(axial, axial.Rotate60().Rotate60().Rotate60().Rotate60().Rotate60().Rotate60());
        Assert.Equal(axial, axial.Rotate120().Rotate120().Rotate120());
        Assert.Equal(axial, axial.Rotate180().Rotate180());
        Assert.Equal(axial, axial.Rotate240().Rotate120());
        Assert.Equal(axial, axial.Rotate300().Rotate60());

        Assert.Equal(axial.Rotate120(), axial.Rotate60().Rotate60());

        Assert.Equal(axial.Rotate180(), axial.Rotate60().Rotate120());
        Assert.Equal(axial.Rotate180(), axial.Rotate120().Rotate60());
        Assert.Equal(axial.Rotate180(), axial.Rotate60().Rotate60().Rotate60());

        Assert.Equal(axial.Rotate240(), axial.Rotate60().Rotate180());
        Assert.Equal(axial.Rotate240(), axial.Rotate120().Rotate120());
        Assert.Equal(axial.Rotate240(), axial.Rotate180().Rotate60());
        Assert.Equal(axial.Rotate240(), axial.Rotate60().Rotate60().Rotate60().Rotate60());

        Assert.Equal(axial.Rotate300(), axial.Rotate60().Rotate240());
        Assert.Equal(axial.Rotate300(), axial.Rotate120().Rotate180());
        Assert.Equal(axial.Rotate300(), axial.Rotate180().Rotate120());
        Assert.Equal(axial.Rotate300(), axial.Rotate240().Rotate60());
        Assert.Equal(axial.Rotate300(), axial.Rotate60().Rotate60().Rotate60().Rotate60().Rotate60());

        Assert.Equal(axial, axial.Rotate60(0));
        Assert.Equal(axial, axial.Rotate60(6));
        Assert.Equal(axial, axial.Rotate60(12));
        Assert.Equal(axial, axial.Rotate60(-6));
        Assert.Equal(axial, axial.Rotate60(-12));

        Assert.Equal(axial.Rotate60(), axial.Rotate60(1));
        Assert.Equal(axial.Rotate120(), axial.Rotate60(2));
        Assert.Equal(axial.Rotate180(), axial.Rotate60(3));
        Assert.Equal(axial.Rotate240(), axial.Rotate60(4));
        Assert.Equal(axial.Rotate300(), axial.Rotate60(5));
    }
}
