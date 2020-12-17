using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Quadrilateral
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

        public Quadrilateral Transform(Matrix4x4 matrix)
        {
            return new Quadrilateral(
                Vector2.Transform(A, matrix),
                Vector2.Transform(B, matrix),
                Vector2.Transform(C, matrix),
                Vector2.Transform(D, matrix));
        }

        public override string ToString() => $"{A} {B} {C} {D}";
    }
}
