using System;
using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Quadrilateral<T> : IEquatable<Quadrilateral<T>> where T : IEquatable<T>
    {
        public readonly T A;
        public readonly T B;
        public readonly T C;
        public readonly T D;

        public Quadrilateral(T abcd)
        {
            A = abcd;
            B = abcd;
            C = abcd;
            D = abcd;
        }

        public Quadrilateral(T a, T b, T c, T d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public bool Equals(Quadrilateral<T> other)
        {
            return
                A.Equals(other.A) &&
                B.Equals(other.B) &&
                C.Equals(other.C) &&
                D.Equals(other.D);
        }

        public override bool Equals(object? obj) => obj is Quadrilateral<T> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(A, B, C, D);
        public override string ToString() => $"{A} {B} {C} {D}";
    }

    public static class Quadrilateral
    {
        public static Quadrilateral<Vector2> Create(Vector2 a, Vector2 c)
        {
            return new Quadrilateral<Vector2>(
                a,
                new Vector2(a.X, c.Y),
                c,
                new Vector2(c.X, a.Y));
        }
    }
}
