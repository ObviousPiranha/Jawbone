using System;

namespace Piranha.Jawbone.Tools;

public static class MathExtensions
{
    public static int Clamped(this int value, int min, int max) => Math.Max(min, Math.Min(max, value));
    public static long Clamped(this long value, long min, long max) => Math.Max(min, Math.Min(max, value));
    public static float Clamped(this float value, float min, float max) => Math.Max(min, Math.Min(max, value));
    public static double Clamped(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));
}
