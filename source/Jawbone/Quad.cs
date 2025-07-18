using System;
using System.Numerics;

namespace Jawbone;

public struct Quad<T>
{
    public T A;
    public T B;
    public T C;
    public T D;

    public Quad(T abcd)
    {
        A = abcd;
        B = abcd;
        C = abcd;
        D = abcd;
    }

    public Quad(T a, T b, T c, T d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public override readonly string ToString() => $"{A} {B} {C} {D}";
}

public static class Quad
{
    public static Quad<T> Create<T>(T abcd) => new(abcd);
    public static Quad<T> Create<T>(T a, T b, T c, T d) => new(a, b, c, d);

    // [Obsolete("Use CreateAxCy instead.")]
    public static Quad<Vector2> Create(Vector2 a, Vector2 c) => CreateAxCy(a, c);

    public static Quad<Vector2> CreateAxCy(Vector2 a, Vector2 c)
    {
        var axcy = new Vector2(a.X, c.Y);
        var cxay = new Vector2(c.X, a.Y);
        return new Quad<Vector2>(a, axcy, c, cxay);
    }

    public static Quad<Vector2> CreateCxAy(Vector2 a, Vector2 c)
    {
        var axcy = new Vector2(a.X, c.Y);
        var cxay = new Vector2(c.X, a.Y);
        return new Quad<Vector2>(a, cxay, c, axcy);
    }

    public static Quad<Vector2> CreateLhr(Vector2 a, Vector2 c)
    {
        // Rotate coordinates using left-handed coordinate system.
        if (a.X < c.X ^ a.Y < c.Y)
        {
            return CreateCxAy(a, c);
        }
        else
        {
            return CreateAxCy(a, c);
        }
    }

    public static Quad<Vector2> CreateRhr(Vector2 a, Vector2 c)
    {
        // Rotate coordinates using right-handed coordinate system.
        if (a.X < c.X ^ a.Y < c.Y)
        {
            return CreateAxCy(a, c);
        }
        else
        {
            return CreateCxAy(a, c);
        }
    }

    public static Quad<T3> Combine<T1, T2, T3>(
        Quad<T1> a,
        Quad<T2> b,
        Func<T1, T2, T3> combiner)
    {
        return new Quad<T3>(
            combiner.Invoke(a.A, b.A),
            combiner.Invoke(a.B, b.B),
            combiner.Invoke(a.C, b.C),
            combiner.Invoke(a.D, b.D));
    }

    public static Quad<T3> Combine<T1, T2, T3, TState>(
        Quad<T1> a,
        Quad<T2> b,
        TState state,
        Func<T1, T2, TState, T3> combiner)
    {
        return new Quad<T3>(
            combiner.Invoke(a.A, b.A, state),
            combiner.Invoke(a.B, b.B, state),
            combiner.Invoke(a.C, b.C, state),
            combiner.Invoke(a.D, b.D, state));
    }

    public static T Min<T>(this Quad<T> quad) where T : INumber<T>
    {
        var result = T.Min(quad.A, T.Min(quad.B, T.Min(quad.C, quad.D)));
        return result;
    }

    public static T Max<T>(this Quad<T> quad) where T : INumber<T>
    {
        var result = T.Max(quad.A, T.Max(quad.B, T.Max(quad.C, quad.D)));
        return result;
    }

    public static (Vector2 min, Vector2 max) MinMax(this Quad<Vector2> quad)
    {
        var min = new Vector2(
            FindMin(quad.A.X, quad.B.X, quad.C.X, quad.D.X),
            FindMin(quad.A.Y, quad.B.Y, quad.C.Y, quad.D.Y));
        var max = new Vector2(
            FindMax(quad.A.X, quad.B.X, quad.C.X, quad.D.X),
            FindMax(quad.A.Y, quad.B.Y, quad.C.Y, quad.D.Y));
        return (min, max);
    }

    public static float MinX(this Quad<Vector2> quad) => FindMin(quad.A.X, quad.B.X, quad.C.X, quad.D.X);
    public static float MaxX(this Quad<Vector2> quad) => FindMax(quad.A.X, quad.B.X, quad.C.X, quad.D.X);

    private static float FindMin(float a, float b, float c, float d) => float.Min(a, float.Min(b, float.Min(c, d)));
    private static float FindMax(float a, float b, float c, float d) => float.Max(a, float.Max(b, float.Max(c, d)));

    public static Quad<TResult> Select<T, TResult>(
        this Quad<T> q,
        Func<T, TResult> f)
    {
        var result = new Quad<TResult>(
            f.Invoke(q.A),
            f.Invoke(q.B),
            f.Invoke(q.C),
            f.Invoke(q.D));

        return result;
    }

    public static Quad<TResult> Select<T, TState, TResult>(
        this Quad<T> q,
        TState state,
        Func<T, TState, TResult> f)
    {
        var result = new Quad<TResult>(
            f.Invoke(q.A, state),
            f.Invoke(q.B, state),
            f.Invoke(q.C, state),
            f.Invoke(q.D, state));

        return result;
    }

    public static Quad<Vector2> Transformed(
        in this Quad<Vector2> q,
        in Matrix4x4 matrix)
    {
        return new Quad<Vector2>(
            Vector2.Transform(q.A, matrix),
            Vector2.Transform(q.B, matrix),
            Vector2.Transform(q.C, matrix),
            Vector2.Transform(q.D, matrix));
    }

    public static Quad<Vector2> Transformed(
        in this Quad<Vector2> q,
        in Matrix3x2 matrix)
    {
        return new Quad<Vector2>(
            Vector2.Transform(q.A, matrix),
            Vector2.Transform(q.B, matrix),
            Vector2.Transform(q.C, matrix),
            Vector2.Transform(q.D, matrix));
    }

    public static Quad<Vector2> Negated(in this Quad<Vector2> q)
    {
        return new Quad<Vector2>(
            -q.A,
            -q.B,
            -q.C,
            -q.D);
    }

    public static Quad<Vector2> NegatedX(in this Quad<Vector2> q)
    {
        return new Quad<Vector2>(
            new Vector2(-q.A.X, q.A.Y),
            new Vector2(-q.B.X, q.B.Y),
            new Vector2(-q.C.X, q.C.Y),
            new Vector2(-q.D.X, q.D.Y));
    }

    public static Quad<Vector2> NegatedY(in this Quad<Vector2> q)
    {
        return new Quad<Vector2>(
            new Vector2(q.A.X, -q.A.Y),
            new Vector2(q.B.X, -q.B.Y),
            new Vector2(q.C.X, -q.C.Y),
            new Vector2(q.D.X, -q.D.Y));
    }

    public static Quad<Vector2> Translated(
        in this Quad<Vector2> q,
        Vector2 offset)
    {
        return new Quad<Vector2>(
            q.A + offset,
            q.B + offset,
            q.C + offset,
            q.D + offset);
    }

    public static bool Contains(
        in this Quad<Vector2> q,
        Vector2 v)
    {
        return
            Passes(q.A, q.B, v) &&
            Passes(q.B, q.C, v) &&
            Passes(q.C, q.D, v) &&
            Passes(q.D, q.A, v);

        static bool Passes(Vector2 origin, Vector2 p1, Vector2 p2)
        {
            return 0f <= Vector2.Dot(p1 - origin, p2 - origin);
        }
    }

    public static Quad<Vector2> RotatedClockwiseAboutOrigin(in this Quad<Vector2> q, int stepCount)
    {
        return (stepCount & 3) switch
        {
            0 => q,
            1 => q.RotatedClockwiseAboutOrigin(),
            2 => q.Negated(),
            3 => q.RotatedCounterclockwiseAboutOrigin(),
            _ => ExceptionHelper.ThrowInvalidValue<Quad<Vector2>>(nameof(stepCount))
        };
    }

    public static Quad<Vector2> RotatedCounterclockwiseAboutOrigin(in this Quad<Vector2> q, int stepCount)
    {
        return (stepCount & 3) switch
        {
            0 => q,
            1 => q.RotatedCounterclockwiseAboutOrigin(),
            2 => q.Negated(),
            3 => q.RotatedClockwiseAboutOrigin(),
            _ => ExceptionHelper.ThrowInvalidValue<Quad<Vector2>>(nameof(stepCount))
        };
    }

    public static Quad<Vector2> RotatedClockwiseAboutOrigin(in this Quad<Vector2> q)
    {
        return new Quad<Vector2>(
            Rotated.Clockwise(q.A),
            Rotated.Clockwise(q.B),
            Rotated.Clockwise(q.C),
            Rotated.Clockwise(q.D));
    }

    public static Quad<Vector2> RotatedCounterclockwiseAboutOrigin(in this Quad<Vector2> q)
    {
        return new Quad<Vector2>(
            Rotated.Counterclockwise(q.A),
            Rotated.Counterclockwise(q.B),
            Rotated.Counterclockwise(q.C),
            Rotated.Counterclockwise(q.D));
    }
}
