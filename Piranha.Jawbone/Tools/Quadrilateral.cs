using System;
using System.Numerics;

namespace Piranha.Jawbone.Tools
{
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
    }
}
