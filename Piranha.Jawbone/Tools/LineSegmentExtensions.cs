namespace Piranha.Jawbone.Tools;

public static class LineSegmentExtensions
{
    private static void Ascend(int a, int b, out int low, out int high)
    {
        if (b < a)
        {
            low = b;
            high = a;
        }
        else
        {
            low = a;
            high = b;
        }
    }

    public static Rectangle32 ToRectangle(this LineSegment32 ls)
    {
        Ascend(ls.A.X, ls.B.X, out var lowX, out var highX);
        Ascend(ls.A.Y, ls.B.Y, out var lowY, out var highY);
        return new Rectangle32(
            new Point32(lowX, lowY),
            new Point32(highX - lowX + 1, highY - lowY + 1));
    }
}
