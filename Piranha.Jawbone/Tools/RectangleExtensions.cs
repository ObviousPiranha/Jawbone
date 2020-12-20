namespace Piranha.Jawbone.Tools
{
    public static class RectangleExtensions
    {
        private static (Rectangle32 rx, Rectangle32 ry) Pack(
            this Rectangle32 r,
            Point32 size,
            int rightHeight,
            int bottomWidth)
        {
            var rx = new Rectangle32(
                new Point32(r.Position.X + size.X, r.Position.Y),
                new Point32(r.Size.X - size.X, rightHeight));
            var ry = new Rectangle32(
                new Point32(r.Position.X, r.Position.Y + size.Y),
                new Point32(bottomWidth, r.Size.Y - size.Y));
            
            return (rx, ry);
        }

        public static (Rectangle32 rx, Rectangle32 ry) PackSmallX(
            this Rectangle32 r, Point32 size) => r.Pack(size, size.Y, r.Size.X);
        public static (Rectangle32 rx, Rectangle32 ry) PackSmallY(
            this Rectangle32 r, Point32 size) => r.Pack(size, r.Size.Y, size.X);
    }
}
