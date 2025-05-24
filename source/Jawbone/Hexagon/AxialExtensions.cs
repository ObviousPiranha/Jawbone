namespace Jawbone.Hexagon;

public static class AxialExtensions
{
    // +Q +R +S
    // -R -S -Q
    // +S +Q +R
    // -Q -R -S
    // +R +S +Q
    // -S -Q -R
    // +Q +R +S

    public static Axial32 Rotate60(this Axial32 a) => new(-a.R, -a.S);
    public static Axial32 Rotate120(this Axial32 a) => new(a.S, a.Q);
    public static Axial32 Rotate180(this Axial32 a) => new(-a.Q, -a.R);
    public static Axial32 Rotate240(this Axial32 a) => new(a.R, a.S);
    public static Axial32 Rotate300(this Axial32 a) => new(-a.S, -a.Q);

    public static Axial32 Rotate60(this Axial32 a, int n)
    {
        var remainder = n % 6;

        if (remainder < 0)
            remainder += 6;

        var result = remainder switch
        {
            1 => a.Rotate60(),
            2 => a.Rotate120(),
            3 => a.Rotate180(),
            4 => a.Rotate240(),
            5 => a.Rotate300(),
            _ => a
        };

        return result;
    }
}
