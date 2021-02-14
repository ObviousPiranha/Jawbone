using System.Numerics;

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
        
        public static Rectangle32 Moved(this Rectangle32 r, Point32 offset) => new(r.Position + offset, r.Size);
        public static Rectangle32 Padded(this Rectangle32 r, Point32 padding) => new(r.Position + padding, r.Size - padding * 2);
        public static Rectangle32 Padded(this Rectangle32 r, int padding) => r.Padded(new Point32(padding, padding));

        public static Quadrilateral<Vector2> ToTextureCoordinates(this Rectangle32 r, Point32 textureSize)
        {
            var w = (float)textureSize.X;
            var h = (float)textureSize.Y;

            return Quadrilateral.Create(
                new Vector2(r.Position.X / w, r.Position.Y / h),
                new Vector2((r.Position.X + r.Size.X) / w, (r.Position.Y + r.Size.Y) / h));
        }

        public static int FarX(this Rectangle32 r) => r.Position.X + r.Size.X;
        public static int FarY(this Rectangle32 r) => r.Position.Y + r.Size.Y;

        public static bool Overlaps(this Rectangle32 r, Rectangle32 other)
        {
            return !(
                r.FarX() <= other.Position.X ||
                other.FarX() <= r.Position.X ||
                r.FarY() <= other.Position.Y ||
                other.FarY() <= r.Position.Y);
        }
    }
}
