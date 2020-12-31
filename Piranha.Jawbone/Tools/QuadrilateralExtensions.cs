using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public static class QuadrilateralExtensions
    {
        public static Quadrilateral Transformed(
            in this Quadrilateral q,
            in Matrix4x4 matrix)
        {
            return new Quadrilateral(
                Vector2.Transform(q.A, matrix),
                Vector2.Transform(q.B, matrix),
                Vector2.Transform(q.C, matrix),
                Vector2.Transform(q.D, matrix));
        }

        public static Quadrilateral Rotated(
            in this Quadrilateral q,
            float radians)
        {
            var matrix = Matrix4x4.CreateRotationZ(radians);
            return q.Transformed(matrix);
        }

        public static Quadrilateral NegatedX(in this Quadrilateral q)
        {
            return new Quadrilateral(
                new Vector2(-q.A.X, q.A.Y),
                new Vector2(-q.B.X, q.B.Y),
                new Vector2(-q.C.X, q.C.Y),
                new Vector2(-q.D.X, q.D.Y));
        }

        public static Quadrilateral NegatedY(in this Quadrilateral q)
        {
            return new Quadrilateral(
                new Vector2(q.A.X, -q.A.Y),
                new Vector2(q.B.X, -q.B.Y),
                new Vector2(q.C.X, -q.C.Y),
                new Vector2(q.D.X, -q.D.Y));
        }

        public static Quadrilateral Translated(
            in this Quadrilateral q,
            Vector2 offset)
        {
            return new Quadrilateral(
                q.A + offset,
                q.B + offset,
                q.C + offset,
                q.D + offset);
        }
    }
}
