namespace Piranha.Jawbone.Tools
{
    public static class PointExtensions
    {
        public static bool HasArea(this Point32 p) => 0 < p.X && 0 < p.Y;
        public static bool HasNegative(this Point32 p) => p.X < 0 || p.Y < 0;
    }
}
