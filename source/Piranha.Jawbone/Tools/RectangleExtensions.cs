using System.Numerics;

namespace Piranha.Jawbone;

public static class RectangleExtensions
{
    public static Rectangle32 Moved(this Rectangle32 r, Point32 offset) => new(r.Position + offset, r.Size);
    public static Rectangle32 Padded(this Rectangle32 r, Point32 padding) => new(r.Position + padding, r.Size - padding * 2);
    public static Rectangle32 Padded(this Rectangle32 r, int padding) => r.Padded(new Point32(padding, padding));

    public static Quad<Vector2> ToTextureCoordinates(this Rectangle32 r, Point32 textureSize)
    {
        var w = (float)textureSize.X;
        var h = (float)textureSize.Y;

        return Quad.Create(
            new Vector2(r.Position.X / w, r.Position.Y / h),
            new Vector2((r.Position.X + r.Size.X) / w, (r.Position.Y + r.Size.Y) / h));
    }

    public static Quad<Point32> ToQuad(this Rectangle32 r)
    {
        return new Quad<Point32>(
            r.Position,
            r.Position + new Point32(0, r.Size.Y),
            r.High(),
            r.Position + new Point32(r.Size.X, 0));
    }

    public static bool Overlaps(this Rectangle32 r, Rectangle32 other)
    {
        return !(
            r.HighX() <= other.Position.X ||
            other.HighX() <= r.Position.X ||
            r.HighY() <= other.Position.Y ||
            other.HighY() <= r.Position.Y);
    }

    public static Point32 High(this Rectangle32 r) => r.Position + r.Size;
    public static Point32 Last(this Rectangle32 r) => r.Position + r.Size - 1;
    public static Point32 LowXLowY(this Rectangle32 r) => r.Position;
    public static Point32 LowXHighY(this Rectangle32 r) => r.Position + new Point32(0, r.Size.Y);
    public static Point32 HighXLowY(this Rectangle32 r) => r.Position + new Point32(r.Size.X, 0);
    public static Point32 HighXHighY(this Rectangle32 r) => r.Position + r.Size;
    public static Point32 LowXLastY(this Rectangle32 r) => r.Position + new Point32(0, r.Size.Y - 1);
    public static Point32 LastXLowY(this Rectangle32 r) => r.Position + new Point32(r.Size.X - 1, 0);
    public static Point32 LastXLastY(this Rectangle32 r) => r.Position + r.Size - 1;
    public static int HighX(this Rectangle32 r) => r.Position.X + r.Size.X;
    public static int HighY(this Rectangle32 r) => r.Position.Y + r.Size.Y;
    public static int LastX(this Rectangle32 r) => r.Position.X + r.Size.X - 1;
    public static int LastY(this Rectangle32 r) => r.Position.Y + r.Size.Y - 1;
    public static int LowX(this Rectangle32 r) => r.Position.X;
    public static int LowY(this Rectangle32 r) => r.Position.Y;
    public static Point32 Middle(this Rectangle32 r) => r.Position + r.Size / 2;
    public static int MiddleX(this Rectangle32 r) => r.Position.X + r.Size.X / 2;
    public static int MiddleY(this Rectangle32 r) => r.Position.Y + r.Size.Y / 2;
}
