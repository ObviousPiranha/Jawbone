using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public static class RectangleExtensions
    {
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

        public static Rectangle32 Shrunk(this Rectangle32 r, int n)
        {
            var offset = new Point32(n, n);
            return new Rectangle32(r.Position + offset, r.Size - offset * 2);
        }

        public static Point32 High(this Rectangle32 r) => r.Position + r.Size;
        public static int HighX(this Rectangle32 r) => r.Position.X + r.Size.X;
        public static int HighY(this Rectangle32 r) => r.Position.Y + r.Size.Y;
    }
}
