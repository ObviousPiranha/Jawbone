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
}
