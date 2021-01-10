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

    public readonly struct Quadrilateral : IEquatable<Quadrilateral>
    {
        public readonly Vector2 A;
        public readonly Vector2 B;
        public readonly Vector2 C;
        public readonly Vector2 D;

        public Quadrilateral(
            Vector2 a,
            Vector2 c)
        {
            A = a;
            B = new Vector2(a.X, c.Y);
            C = c;
            D = new Vector2(c.X, a.Y);
        }

        public Quadrilateral(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Vector2 d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public bool Equals(Quadrilateral other)
        {
            return
                A.Equals(other.A) &&
                B.Equals(other.B) &&
                C.Equals(other.C) &&
                D.Equals(other.D);
        }

        public override bool Equals(object? obj) => obj is Quadrilateral other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(A, B, C, D);
        public override string ToString() => $"{A} {B} {C} {D}";

        public static Quadrilateral operator -(in Quadrilateral q) => new(-q.A, -q.B, -q.C, -q.D);
    }
}
