using System;
using System.Numerics;

namespace Piranha.Jawbone;

public readonly struct Quad<T> : IEquatable<Quad<T>> where T : IEquatable<T>
{
    public readonly T A;
    public readonly T B;
    public readonly T C;
    public readonly T D;

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

    public bool Equals(Quad<T> other)
    {
        return
            A.Equals(other.A) &&
            B.Equals(other.B) &&
            C.Equals(other.C) &&
            D.Equals(other.D);
    }

    public override bool Equals(object? obj) => obj is Quad<T> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(A, B, C, D);
    public override string ToString() => $"{A} {B} {C} {D}";

    public static bool operator ==(Quad<T> a, Quad<T> b) => a.Equals(b);
    public static bool operator !=(Quad<T> a, Quad<T> b) => !a.Equals(b);
}

public static class Quad
{
    public static Quad<Vector2> Create(Vector2 a, Vector2 c)
    {
        return new Quad<Vector2>(
            a,
            new Vector2(a.X, c.Y),
            c,
            new Vector2(c.X, a.Y));
    }

    public static Quad<TResult> Change<T, TArg, TResult>(
        this Quad<T> q,
        TArg arg,
        Func<T, TArg, TResult> f
        ) where T : IEquatable<T> where TResult : IEquatable<TResult>
    {
        var result = new Quad<TResult>(
            f.Invoke(q.A, arg),
            f.Invoke(q.B, arg),
            f.Invoke(q.C, arg),
            f.Invoke(q.D, arg));

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

    public static Quad<Vector2> Rotated(
        in this Quad<Vector2> q,
        float radians)
    {
        var matrix = Matrix3x2.CreateRotation(radians);
        return q.Transformed(matrix);
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
            RotatedClockwise(q.A),
            RotatedClockwise(q.B),
            RotatedClockwise(q.C),
            RotatedClockwise(q.D));
    }

    public static Quad<Vector2> RotatedCounterclockwiseAboutOrigin(in this Quad<Vector2> q)
    {
        return new Quad<Vector2>(
            RotatedCounterclockwise(q.A),
            RotatedCounterclockwise(q.B),
            RotatedCounterclockwise(q.C),
            RotatedCounterclockwise(q.D));
    }

    private static Vector2 RotatedClockwise(Vector2 vector) => new(vector.Y, -vector.X);
    private static Vector2 RotatedCounterclockwise(Vector2 vector) => new(-vector.Y, vector.X);
}