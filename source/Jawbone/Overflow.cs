namespace Jawbone;

public static class Overflow
{
    public static int Add(int a, int b)
    {
        return unchecked((int)((uint)a + (uint)b));
    }

    public static void Add(ref int a, int b) => a = Add(a, b);

    public static Point32 Add(Point32 a, Point32 b)
    {
        return new Point32(
            Add(a.X, b.X),
            Add(a.Y, b.Y));
    }

    public static void Add(ref Point32 a, Point32 b) => a = Add(a, b);
}