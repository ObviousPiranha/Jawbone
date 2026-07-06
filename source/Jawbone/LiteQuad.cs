using System.Numerics;

namespace Jawbone;

/// <summary>
/// Represents a quad using only three points.
/// Point D is derived by adding B->A to C (or B->C to A);
/// </summary>
public struct LiteQuad<T>
{
    public T A;
    public T B;
    public T C;

    public LiteQuad(T abc)
    {
        A = abc;
        B = abc;
        C = abc;
    }

    public LiteQuad(T a, T b, T c)
    {
        A = a;
        B = b;
        C = c;
    }

    public override readonly string ToString() => $"{A} {B} {C}";
}

public static class LiteQuad
{
    public static LiteQuad<T> Create<T>(T abc) => new(abc);
    public static LiteQuad<T> Create<T>(T a, T b, T c) => new(a, b, c);
    public static LiteQuad<T> Create<T>(Quad<T> q) => new(q.A, q.B, q.C);

    public static LiteQuad<Vector2> Transformed(LiteQuad<Vector2> lq, Matrix3x2 m) =>
        new(Vector2.Transform(lq.A, m), Vector2.Transform(lq.B, m), Vector2.Transform(lq.C, m));

    public static LiteQuad<Vector2> Translated(LiteQuad<Vector2> lq, Vector2 v) => new(lq.A + v, lq.B + v, lq.C + v);
}
