using System;
using System.Numerics;

namespace Jawbone;

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

    // [Obsolete("Use ToQuadAxCy instead.")]
    public static Quad<Vector2> ToQuad(this AaQuad<Vector2> aaq) => Quad.Create(aaq.A, aaq.C);
    public static Quad<Vector2> ToQuadAxCy(this AaQuad<Vector2> aaq) => Quad.CreateAxCy(aaq.A, aaq.C);
    public static Quad<Vector2> ToQuadCxAy(this AaQuad<Vector2> aaq) => Quad.CreateCxAy(aaq.A, aaq.C);
    public static Quad<Vector2> ToQuadLhr(this AaQuad<Vector2> aaq) => Quad.CreateLhr(aaq.A, aaq.C);
    public static Quad<Vector2> ToQuadRhr(this AaQuad<Vector2> aaq) => Quad.CreateRhr(aaq.A, aaq.C);

    public static Quad<Vector3> ToQuad(this AaQuad<Vector3> aaq)
    {
        var z = aaq.A.Z;
        return new Quad<Vector3>(
            aaq.A,
            new(aaq.A.X, aaq.C.Y, z),
            aaq.C,
            new(aaq.C.X, aaq.A.Y, z));
    }

    public static AaQuad<TResult> Select<T, TResult>(this AaQuad<T> aaQuad, Func<T, TResult> converter)
    {
        var result = Create(
            converter.Invoke(aaQuad.A),
            converter.Invoke(aaQuad.C));

        return result;
    }

    public static AaQuad<Vector2> ToAaTextureCoordinates(this Rectangle32 r, Point32 textureSize)
    {
        var w = (float)textureSize.X;
        var h = (float)textureSize.Y;

        return Create(
            new Vector2(r.Position.X / w, r.Position.Y / h),
            new Vector2(r.HighX() / w, r.HighY() / h));
    }

    public static Vector2 Size(this AaQuad<Vector2> aaq) => Vector2.Abs(aaq.C - aaq.A);
    public static float Width(this AaQuad<Vector2> aaq) => aaq.C.X - aaq.A.X;
    public static float Width(this AaQuad<Vector3> aaq) => aaq.C.X - aaq.A.X;
    public static float Height(this AaQuad<Vector2> aaq) => aaq.C.Y - aaq.A.Y;
    public static float Height(this AaQuad<Vector3> aaq) => aaq.C.Y - aaq.A.Y;
    public static float XyRatio(this AaQuad<Vector2> aaq) => aaq.Width() / aaq.Height();
    public static float XyRatio(this AaQuad<Vector3> aaq) => aaq.Width() / aaq.Height();
}
