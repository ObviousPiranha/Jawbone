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

        public static Quadrilateral RotatedClockwiseAboutOrigin(in this Quadrilateral q, int stepCount)
        {
            return (stepCount & 3) switch
            {
                0 => q,
                1 => q.RotatedClockwiseAboutOrigin(),
                2 => -q,
                3 => q.RotatedCounterclockwiseAboutOrigin(),
                _ => ExceptionHelper.ThrowInvalidValue<Quadrilateral>(nameof(stepCount))
            };
        }

        public static Quadrilateral RotatedCounterclockwiseAboutOrigin(in this Quadrilateral q, int stepCount)
        {
            return (stepCount & 3) switch
            {
                0 => q,
                1 => q.RotatedCounterclockwiseAboutOrigin(),
                2 => -q,
                3 => q.RotatedClockwiseAboutOrigin(),
                _ => ExceptionHelper.ThrowInvalidValue<Quadrilateral>(nameof(stepCount))
            };
        }

        public static Quadrilateral RotatedClockwiseAboutOrigin(in this Quadrilateral q)
        {
            return new Quadrilateral(
                RotatedClockwise(q.A),
                RotatedClockwise(q.B),
                RotatedClockwise(q.C),
                RotatedClockwise(q.D));
        }

        public static Quadrilateral RotatedCounterclockwiseAboutOrigin(in this Quadrilateral q)
        {
            return new Quadrilateral(
                RotatedCounterclockwise(q.A),
                RotatedCounterclockwise(q.B),
                RotatedCounterclockwise(q.C),
                RotatedCounterclockwise(q.D));
        }

        private static Vector2 RotatedClockwise(Vector2 vector) => new(vector.Y, -vector.X);
        private static Vector2 RotatedCounterclockwise(Vector2 vector) => new(-vector.Y, vector.X);
    }
}
