using System;

namespace Piranha.Jawbone.Tools
{
    public static class PointExtensions
    {
        public static bool AllPositive(this Point32 p) => 0 < p.X && 0 < p.Y;
        public static bool AnyNegative(this Point32 p) => p.X < 0 || p.Y < 0;
        public static int Min(this Point32 p) => Math.Min(p.X, p.Y);
        public static int Max(this Point32 p) => Math.Max(p.X, p.Y);
        
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
    }
}
