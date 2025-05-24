using System;

namespace Jawbone;

public static class IntegerMath
{
    public static int SquareRoot32(double n) => (int)double.Floor(double.Sqrt(n));
    public static long SquareRoot64(double n) => (long)double.Floor(double.Sqrt(n));
}
