using System;

namespace Piranha.Jawbone.Extensions;

public static class RandomExtensions
{
    public static bool NextBoolean(this Random random) => random.Next(2) == 0;
    public static bool NextBoolean(this Random random, int odds) => random.Next(odds) == 0;
    public static double BoxMullerTransform(this Random random, double mean, double standardDeviation)
    {
        return mean + BoxMullerTransform(random, standardDeviation);
    }

    public static double BoxMullerTransform(this Random random, double standardDeviation)
    {
        // https://stackoverflow.com/a/218600
        var u1 = 1.0 - random.NextDouble();
        var u2 = 1.0 - random.NextDouble();
        var randStdNormal = double.Sqrt(-2.0 * double.Log(u1)) * double.Sin(2.0 * double.Pi * u2);
        var result = standardDeviation * randStdNormal;
        return result;
    }
}
