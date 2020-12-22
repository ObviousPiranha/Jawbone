namespace Piranha.Jawbone.Tools
{
    public static class RectangleExtensions
    {
        private static void InnerPack(
            this Rectangle32 r,
            Point32 size,
            int rightHeight,
            int bottomWidth,
            out Rectangle32 rx,
            out Rectangle32 ry)
        {
            rx = new Rectangle32(
                new Point32(r.Position.X + size.X, r.Position.Y),
                new Point32(r.Size.X - size.X, rightHeight));
            ry = new Rectangle32(
                new Point32(r.Position.X, r.Position.Y + size.Y),
                new Point32(bottomWidth, r.Size.Y - size.Y));
        }

        public static void Pack(
            this Rectangle32 r,
            Point32 size,
            out Rectangle32 rx,
            out Rectangle32 ry)
        {
            var fit = r.Size - size;

            if (fit.X < fit.Y)
            {
                r.PackSmallX(size, out rx, out ry);
            }
            else
            {
                r.PackSmallY(size, out rx, out ry);
            }
        }

        public static void PackSmallX(
            this Rectangle32 r,
            Point32 size,
            out Rectangle32 rx,
            out Rectangle32 ry) => r.InnerPack(size, size.Y, r.Size.X, out rx, out ry);
        public static void PackSmallY(
            this Rectangle32 r,
            Point32 size,
            out Rectangle32 rx,
            out Rectangle32 ry) => r.InnerPack(size, r.Size.Y, size.X, out rx, out ry);
    }
}
