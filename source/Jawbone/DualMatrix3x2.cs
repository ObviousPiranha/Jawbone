using System.Numerics;

namespace Jawbone;

public readonly struct DualMatrix3x2
{
    public readonly Matrix3x2 Forward;
    public readonly Matrix3x2 Inverse;

    public DualMatrix3x2(Matrix3x2 forward, Matrix3x2 inverse)
    {
        Forward = forward;
        Inverse = inverse;
    }

    public static DualMatrix3x2 Identity => new(Matrix3x2.Identity, Matrix3x2.Identity);

    public static DualMatrix3x2 CreateRotation(float radians)
    {
        return new(
            Matrix3x2.CreateRotation(radians),
            Matrix3x2.CreateRotation(-radians));
    }

    public static DualMatrix3x2 CreateRotation(float radians, Vector2 centerPoint)
    {
        return new(
            Matrix3x2.CreateRotation(radians, centerPoint),
            Matrix3x2.CreateRotation(-radians, centerPoint));
    }

    public static DualMatrix3x2 CreateScale(float scale)
    {
        return new(
            Matrix3x2.CreateScale(scale),
            Matrix3x2.CreateScale(1f / scale));
    }

    public static DualMatrix3x2 CreateScale(float xScale, float yScale)
    {
        return new(
            Matrix3x2.CreateScale(xScale, yScale),
            Matrix3x2.CreateScale(1f / xScale, 1f / yScale));
    }

    public static DualMatrix3x2 CreateScale(Vector2 scales) => CreateScale(scales.X, scales.Y);

    public static DualMatrix3x2 CreateSkew(float radiansX, float radiansY)
    {
        return new(
            Matrix3x2.CreateSkew(radiansX, radiansY),
            Matrix3x2.CreateSkew(-radiansX, -radiansY));
    }

    public static DualMatrix3x2 CreateSkew(float radiansX, float radiansY, Vector2 centerPoint)
    {
        return new(
            Matrix3x2.CreateSkew(radiansX, radiansY, centerPoint),
            Matrix3x2.CreateSkew(-radiansX, -radiansY, centerPoint));
    }

    public static DualMatrix3x2 CreateTranslation(float xPosition, float yPosition)
    {
        return new(
            Matrix3x2.CreateTranslation(xPosition, yPosition),
            Matrix3x2.CreateTranslation(-xPosition, -yPosition));
    }

    public static DualMatrix3x2 CreateTranslation(Vector2 position)
    {
        return new(
            Matrix3x2.CreateTranslation(position),
            Matrix3x2.CreateTranslation(-position));
    }
    
    public static DualMatrix3x2 operator *(DualMatrix3x2 a, DualMatrix3x2 b) =>
        new(a.Forward * b.Forward, b.Inverse * a.Inverse);
}
