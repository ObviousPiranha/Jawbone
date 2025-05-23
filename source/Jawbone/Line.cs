namespace Jawbone;

public static class Line
{
    private static bool CanSafelyCast(long n) => int.MinValue <= n && n <= int.MaxValue;
    public static bool Intersects(
        int x1,
        int y1,
        int x2,
        int y2,
        int x3,
        int y3,
        int x4,
        int y4,
        out int x,
        out int y)
    {
        if (Intersects64(x1, y1, x2, y2, x3, y3, x4, y4, out var xx, out var yy) &&
            CanSafelyCast(xx) &&
            CanSafelyCast(yy))
        {
            x = (int)xx;
            y = (int)yy;
            return true;
        }
        else
        {
            x = 0;
            y = 0;
            return false;
        }
    }

    public static bool Intersects64(
        long x1,
        long y1,
        long x2,
        long y2,
        long x3,
        long y3,
        long x4,
        long y4,
        out long x,
        out long y)
    {
        // This method finds the intersection regardless of whether it falls on the provided line segments.
        // They are both treated as infinite lines.
        // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Given_two_points_on_each_line

        var d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

        if (d == 0)
        {
            // Lines are parallel.
            x = 0;
            y = 0;
            return false;
        }

        var x1y2_y1x2 = x1 * y2 - y1 * x2;
        var x3y4_y3x4 = x3 * y4 - y3 * x4;

        var xn = x1y2_y1x2 * (x3 - x4) - (x1 - x2) * x3y4_y3x4;
        var yn = x1y2_y1x2 * (y3 - y4) - (y1 - y2) * x3y4_y3x4;
        x = xn / d;
        y = yn / d;
        return true;
    }

    public static bool IntersectsAtX(
        Point32 p1,
        Point32 p2,
        int x,
        out int y)
    {
        return IntersectsAtX(
            p1.X,
            p1.Y,
            p2.X,
            p2.Y,
            x,
            out y);
    }

    public static bool IntersectsAtX(
        int x1,
        int y1,
        int x2,
        int y2,
        int x,
        out int y)
    {
        if (IntersectsAtX64(x1, y1, x2, y2, x, out var yy) && CanSafelyCast(yy))
        {
            y = (int)yy;
            return true;
        }
        else
        {
            y = 0;
            return false;
        }
    }

    public static bool IntersectsAtX64(
        long x1,
        long y1,
        long x2,
        long y2,
        long x,
        out long y)
    {
        // x3 = x
        // y3 = 0
        // x4 = x
        // y4 = 1

        var d = x2 - x1;

        if (d == 0)
        {
            // Lines are parallel.
            y = 0;
            return false;
        }

        var yn = (y1 * x2 - x1 * y2) - (y1 - y2) * x;
        y = yn / d;
        return true;
    }

    public static bool IntersectsAtY(
        Point32 p1,
        Point32 p2,
        int y,
        out int x)
    {
        return IntersectsAtY(
            p1.X,
            p1.Y,
            p2.X,
            p2.Y,
            y,
            out x);
    }

    public static bool IntersectsAtY(
        int x1,
        int y1,
        int x2,
        int y2,
        int y,
        out int x)
    {
        if (IntersectsAtY64(x1, y1, x2, y2, y, out var xx) && CanSafelyCast(xx))
        {
            x = (int)xx;
            return true;
        }
        else
        {
            x = 0;
            return false;
        }
    }

    public static bool IntersectsAtY64(
        long x1,
        long y1,
        long x2,
        long y2,
        long y,
        out long x)
    {
        // x3 = 0
        // y3 = y
        // x4 = 1
        // y4 = y

        var d = y1 - y2;

        if (d == 0)
        {
            // Lines are parallel.
            x = 0;
            y = 0;
            return false;
        }

        var xn = (y1 * x2 - x1 * y2) + (x1 - x2) * y;
        x = xn / d;
        return true;
    }

    public static bool Intersects(LineSegment32 a, LineSegment32 b, out Point32 intersection)
    {
        return Intersects(
            a.A.X,
            a.A.Y,
            a.B.X,
            a.B.Y,
            b.A.X,
            b.A.Y,
            b.B.X,
            b.B.Y,
            out intersection.X,
            out intersection.Y);
    }

    public static bool IntersectsAtX(LineSegment32 ls, int x, out int y)
    {
        return IntersectsAtX(
            ls.A.X,
            ls.A.Y,
            ls.B.X,
            ls.B.Y,
            x,
            out y);
    }

    public static bool IntersectsAtY(LineSegment32 ls, int y, out int x)
    {
        return IntersectsAtY(
            ls.A.X,
            ls.A.Y,
            ls.B.X,
            ls.B.Y,
            y,
            out x);
    }
}
