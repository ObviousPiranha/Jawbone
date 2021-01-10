using System;

namespace Piranha.Jawbone.Tools
{
    public static class IntegerMath
    {
        public static int SquareRoot32(double n) => (int)Math.Floor(Math.Sqrt(n));
        public static long SquareRoot64(double n) => (long)Math.Floor(Math.Sqrt(n));
    }
}
