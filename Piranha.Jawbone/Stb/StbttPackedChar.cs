using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Stb;

[StructLayout(LayoutKind.Sequential)]
public struct StbttPackedChar
{
    public short X0;
    public short Y0;
    public short X1;
    public short Y1;
    public float XOff;
    public float YOff;
    public float XAdvance;
    public float XOff2;
    public float YOff2;

    public readonly Quad<Vector2> GetCoordinates(Point32 sheetSize)
    {
        var width = (float)sheetSize.X;
        var height = (float)sheetSize.Y;
        return Quad.Create(
            new Vector2(X0 / width, Y0 / height),
            new Vector2(X1 / width, Y1 / height));
    }
}
