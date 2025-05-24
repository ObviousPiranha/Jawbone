using System;

namespace Jawbone;

public enum Orientation
{
    None, // ABCD
    R1, // DABC
    R2, // CDAB
    R3, // BCDA
    F, // ADCB
    FR1, // DCBA
    FR2, // CBAD
    FR3 // BADC
}

public static class OrientationExtensions
{
    private static Orientation ThrowBadOrientation() => throw new ArgumentOutOfRangeException();

    public static Orientation Rotate1(this Orientation orientation)
    {
        var result = orientation switch
        {
            Orientation.None => Orientation.R1,
            Orientation.R1 => Orientation.R2,
            Orientation.R2 => Orientation.R3,
            Orientation.R3 => Orientation.None,
            Orientation.F => Orientation.FR3,
            Orientation.FR1 => Orientation.F,
            Orientation.FR2 => Orientation.FR1,
            Orientation.FR3 => Orientation.FR2,
            _ => ThrowBadOrientation()
        };

        return result;
    }

    public static Orientation Rotate2(this Orientation orientation)
    {
        var result = orientation switch
        {
            Orientation.None => Orientation.R2,
            Orientation.R1 => Orientation.R3,
            Orientation.R2 => Orientation.None,
            Orientation.R3 => Orientation.R1,
            Orientation.F => Orientation.FR2,
            Orientation.FR1 => Orientation.FR3,
            Orientation.FR2 => Orientation.F,
            Orientation.FR3 => Orientation.FR1,
            _ => ThrowBadOrientation()
        };

        return result;
    }

    public static Orientation Rotate3(this Orientation orientation)
    {
        var result = orientation switch
        {
            Orientation.None => Orientation.R3,
            Orientation.R1 => Orientation.None,
            Orientation.R2 => Orientation.R1,
            Orientation.R3 => Orientation.R2,
            Orientation.F => Orientation.FR1,
            Orientation.FR1 => Orientation.FR2,
            Orientation.FR2 => Orientation.FR3,
            Orientation.FR3 => Orientation.F,
            _ => ThrowBadOrientation()
        };

        return result;
    }

    public static Orientation FlipH(this Orientation orientation)
    {
        var result = orientation switch
        {
            Orientation.None => Orientation.FR3,
            Orientation.R1 => Orientation.F,
            Orientation.R2 => Orientation.FR1,
            Orientation.R3 => Orientation.FR2,
            Orientation.F => Orientation.R1,
            Orientation.FR1 => Orientation.R2,
            Orientation.FR2 => Orientation.R3,
            Orientation.FR3 => Orientation.None,
            _ => ThrowBadOrientation()
        };

        return result;
    }

    public static Orientation FlipV(this Orientation orientation)
    {
        var result = orientation switch
        {
            Orientation.None => Orientation.FR1,
            Orientation.R1 => Orientation.FR2,
            Orientation.R2 => Orientation.FR3,
            Orientation.R3 => Orientation.F,
            Orientation.F => Orientation.R3,
            Orientation.FR1 => Orientation.None,
            Orientation.FR2 => Orientation.R1,
            Orientation.FR3 => Orientation.R2,
            _ => ThrowBadOrientation()
        };

        return result;
    }
}
