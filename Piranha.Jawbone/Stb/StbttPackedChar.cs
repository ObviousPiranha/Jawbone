using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Stb;

[StructLayout(LayoutKind.Sequential)]
public struct StbttPackedChar
{
    public short x0;
    public short y0;
    public short x1;
    public short y1;
    public float xOff;
    public float yOff;
    public float xAdvance;
    public float xOff2;
    public float yOff2;

    public Quad<Vector2> GetCoordinates(Point32 sheetSize)
    {
        var width = (float)sheetSize.X;
        var height = (float)sheetSize.Y;
        return Quad.Create(
            new Vector2(x0 / width, y0 / height),
            new Vector2(x1 / width, y1 / height));
    }
}
