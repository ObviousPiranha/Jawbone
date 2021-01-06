namespace Piranha.Jawbone.Tools
{
    public static class CircleExtensions
    {
        public static Rectangle32 ToRectangle(this Circle32 c)
        {
            var edge = c.Radius * 2;
            return new Rectangle32(
                c.Center.Moved(-c.Radius, -c.Radius),
                new Point32(edge + 1, edge + 1));
        }
    }
}
