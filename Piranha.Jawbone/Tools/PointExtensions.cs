using System;
using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public static class PointExtensions
    {
        public static bool AllPositive(this Point32 p) => 0 < p.X && 0 < p.Y;
        public static bool AnyNegative(this Point32 p) => p.X < 0 || p.Y < 0;
        public static int Min(this Point32 p) => Math.Min(p.X, p.Y);
        public static int Max(this Point32 p) => Math.Max(p.X, p.Y);
        public static Point32 Moved(this Point32 p, int dx, int dy) => new(p.X + dx, p.Y + dy);
        
        public static void InOrder(this Point32 p, out int low, out int high)
        {
            if (p.X < p.Y)
            {
                low = p.X;
                high = p.Y;
            }
            else
            {
                low = p.Y;
                high = p.X;
            }
        }

        public static Vector2 ToRatio(this Point32 p)
        {
            if (p.X < p.Y)
            {
                return new Vector2(1f, p.Y / (float)p.X);
            }
            else
            {
                return new Vector2(p.X / (float)p.Y, 1f);
            }
        }

        public static Vector2 ToVector2(this Point32 p) => new(p.X, p.Y);
        public static Point32 Rotated(this Point32 p) => new(p.Y, -p.X);
        public static int LengthSquared(this Point32 p) => p.X * p.X + p.Y * p.Y;
        public static int Length(this Point32 point)
        {
            return IntegerMath.SquareRoot32(
                (double)point.X * point.X + (double)point.Y * point.Y);
        }
    }
}
