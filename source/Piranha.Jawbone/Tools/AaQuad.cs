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
    public static Quad<Vector2> ToQuad(this AaQuad<Vector2> aaq)
    {
        return new Quad<Vector2>(
            aaq.A,
            new Vector2(aaq.A.X, aaq.C.Y),
            aaq.C,
            new Vector2(aaq.C.X, aaq.A.Y));
    }
}
