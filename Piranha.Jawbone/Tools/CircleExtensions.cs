namespace Piranha.Jawbone.Tools
{
    public static class CircleExtensions
    {
        public static Rectangle32 ToRectangle(this Circle32 c)
        {
            var edge = c.Radius * 2 + 1;
            return new Rectangle32(
                c.Center.Moved(-c.Radius, -c.Radius),
                new Point32(edge, edge));
        }

        public static bool Contains(this Circle32 c, Point32 point)
        {
            var delta = point - c.Center;
            var radiusSquared = c.Radius * c.Radius;
            var distanceSquared = delta.LengthSquared();
            return distanceSquared <= radiusSquared;
        }
    }
}
