using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Jawbone;

public readonly struct DualMatrix3x2
{
    public Matrix3x2 Forward { get; init; }
    public Matrix3x2 Inverse { get; init; }

    public DualMatrix3x2(Matrix3x2 forward, Matrix3x2 inverse)
    {
        Forward = forward;
        Inverse = inverse;
    }

    public DualMatrix3x2 Transform(Matrix3x2 forward, Matrix3x2 inverse) =>
        new(Forward * forward, inverse * Inverse);

    public DualMatrix3x2 Scale(float x, float y)
    {
        Validate(x);
        Validate(y);

        return Transform(
            Matrix3x2.CreateScale(x, y),
            Matrix3x2.CreateScale(1f / x, 1f / y));
        
        static void Validate(
            float n,
            [CallerArgumentExpression(nameof(n))] string? paramName = null)
        {
            if (!float.IsRealNumber(n) || float.Abs(n) < float.Epsilon)
                throw new ArgumentException("Scale factor must be real non-zero value.", paramName);
        }
    }

    public DualMatrix3x2 Scale(Vector2 v) => Scale(v.X, v.Y);

    public DualMatrix3x2 Translate(float x, float y)
    {
        return Transform(
            Matrix3x2.CreateTranslation(x, y),
            Matrix3x2.CreateTranslation(-x, -y));
    }

    public DualMatrix3x2 Translate(Vector2 v) => Translate(v.X, v.Y);

    public DualMatrix3x2 Rotate(float radians)
    {
        return Transform(
            Matrix3x2.CreateRotation(radians),
            Matrix3x2.CreateRotation(-radians));
    }

    public static DualMatrix3x2 Start() => new(Matrix3x2.Identity, Matrix3x2.Identity);
}