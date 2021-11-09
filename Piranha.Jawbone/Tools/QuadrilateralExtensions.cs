using System.Numerics;

namespace Piranha.Jawbone.Tools
{
    public static class QuadrilateralExtensions
    {
        public static Quadrilateral<Vector2> Transformed(
            in this Quadrilateral<Vector2> q,
            in Matrix4x4 matrix)
        {
            return new Quadrilateral<Vector2>(
                Vector2.Transform(q.A, matrix),
                Vector2.Transform(q.B, matrix),
                Vector2.Transform(q.C, matrix),
                Vector2.Transform(q.D, matrix));
        }

        public static Quadrilateral<Vector2> Transformed(
            in this Quadrilateral<Vector2> q,
            in Matrix3x2 matrix)
        {
            return new Quadrilateral<Vector2>(
                Vector2.Transform(q.A, matrix),
                Vector2.Transform(q.B, matrix),
                Vector2.Transform(q.C, matrix),
                Vector2.Transform(q.D, matrix));
        }

        public static Quadrilateral<Vector2> Rotated(
            in this Quadrilateral<Vector2> q,
            float radians)
        {
            var matrix = Matrix3x2.CreateRotation(radians);
            return q.Transformed(matrix);
        }

        public static Quadrilateral<Vector2> Negated(in this Quadrilateral<Vector2> q)
        {
            return new Quadrilateral<Vector2>(
                -q.A,
                -q.B,
                -q.C,
                -q.D);
        }

        public static Quadrilateral<Vector2> NegatedX(in this Quadrilateral<Vector2> q)
        {
            return new Quadrilateral<Vector2>(
                new Vector2(-q.A.X, q.A.Y),
                new Vector2(-q.B.X, q.B.Y),
                new Vector2(-q.C.X, q.C.Y),
                new Vector2(-q.D.X, q.D.Y));
        }

        public static Quadrilateral<Vector2> NegatedY(in this Quadrilateral<Vector2> q)
        {
            return new Quadrilateral<Vector2>(
                new Vector2(q.A.X, -q.A.Y),
                new Vector2(q.B.X, -q.B.Y),
                new Vector2(q.C.X, -q.C.Y),
                new Vector2(q.D.X, -q.D.Y));
        }

        public static Quadrilateral<Vector2> Translated(
            in this Quadrilateral<Vector2> q,
            Vector2 offset)
        {
            return new Quadrilateral<Vector2>(
                q.A + offset,
                q.B + offset,
                q.C + offset,
                q.D + offset);
        }

        public static Quadrilateral<Vector2> RotatedClockwiseAboutOrigin(in this Quadrilateral<Vector2> q, int stepCount)
        {
            return (stepCount & 3) switch
            {
                0 => q,
                1 => q.RotatedClockwiseAboutOrigin(),
                2 => q.Negated(),
                3 => q.RotatedCounterclockwiseAboutOrigin(),
                _ => ExceptionHelper.ThrowInvalidValue<Quadrilateral<Vector2>>(nameof(stepCount))
            };
        }

        public static Quadrilateral<Vector2> RotatedCounterclockwiseAboutOrigin(in this Quadrilateral<Vector2> q, int stepCount)
        {
            return (stepCount & 3) switch
            {
                0 => q,
                1 => q.RotatedCounterclockwiseAboutOrigin(),
                2 => q.Negated(),
                3 => q.RotatedClockwiseAboutOrigin(),
                _ => ExceptionHelper.ThrowInvalidValue<Quadrilateral<Vector2>>(nameof(stepCount))
            };
        }

        public static Quadrilateral<Vector2> RotatedClockwiseAboutOrigin(in this Quadrilateral<Vector2> q)
        {
            return new Quadrilateral<Vector2>(
                RotatedClockwise(q.A),
                RotatedClockwise(q.B),
                RotatedClockwise(q.C),
                RotatedClockwise(q.D));
        }

        public static Quadrilateral<Vector2> RotatedCounterclockwiseAboutOrigin(in this Quadrilateral<Vector2> q)
        {
            return new Quadrilateral<Vector2>(
                RotatedCounterclockwise(q.A),
                RotatedCounterclockwise(q.B),
                RotatedCounterclockwise(q.C),
                RotatedCounterclockwise(q.D));
        }

        private static Vector2 RotatedClockwise(Vector2 vector) => new(vector.Y, -vector.X);
        private static Vector2 RotatedCounterclockwise(Vector2 vector) => new(-vector.Y, vector.X);
    }
}
