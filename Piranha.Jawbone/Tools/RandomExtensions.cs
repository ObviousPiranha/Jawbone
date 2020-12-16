using System;

namespace Piranha.Jawbone.Tools.RandomExtensions
{
    public static class RandomExtensions
    {
        public static double BoxMullerTransform(this Random random, double mean, double standardDeviation)
        {
            return mean + BoxMullerTransform(random, standardDeviation);
        }
        
        public static double BoxMullerTransform(this Random random, double standardDeviation)
        {
            // https://stackoverflow.com/a/218600
            var u1 = 1.0 - random.NextDouble();
            var u2 = 1.0 - random.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var result = standardDeviation * randStdNormal;
            return result;
        }
    }
}
