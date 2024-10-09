using System.Numerics;

namespace Piranha.Jawbone;

public struct AaQuad<T> // axis-aligned quad
{
    public T A;
    public T C;

    public AaQuad(T ac)
    {
        A = ac;
        C = ac;
    }

    public AaQuad(T a, T c)
    {
        A = a;
        C = c;
    }

    public override readonly string ToString() => $"{A} {C}";
}

public static class AaQuad
{
    public static AaQuad<T> Create<T>(T a, T c) => new(a, c);
    public static Quad<Vector2> ToQuad(this AaQuad<Vector2> aaq)
    {
        return new Quad<Vector2>(
            aaq.A,
            new(aaq.A.X, aaq.C.Y),
            aaq.C,
            new(aaq.C.X, aaq.A.Y));
    }

    public static Quad<Vector3> ToQuad(this AaQuad<Vector3> aaq)
    {
        var z = aaq.A.Z;
        return new Quad<Vector3>(
            aaq.A,
            new(aaq.A.X, aaq.C.Y, z),
            aaq.C,
            new(aaq.C.X, aaq.A.Y, z));
    }

    public static Vector2 Size(this AaQuad<Vector2> aaq) => aaq.C - aaq.A;
    public static float Width(this AaQuad<Vector2> aaq) => aaq.C.X - aaq.A.X;
    public static float Width(this AaQuad<Vector3> aaq) => aaq.C.X - aaq.A.X;
    public static float Height(this AaQuad<Vector2> aaq) => aaq.C.Y - aaq.A.Y;
    public static float Height(this AaQuad<Vector3> aaq) => aaq.C.Y - aaq.A.Y;
    public static float XyRatio(this AaQuad<Vector2> aaq) => aaq.Width() / aaq.Height();
    public static float XyRatio(this AaQuad<Vector3> aaq) => aaq.Width() / aaq.Height();
}
