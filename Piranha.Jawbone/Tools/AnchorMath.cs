using System;
using System.Numerics;

namespace Piranha.Jawbone;

public static class AnchorMath
{
    public static Matrix3x2 Align(
        Vector2 a,
        Vector2 b,
        Vector2 c,
        Vector2 d)
    {
        // https://stackoverflow.com/a/42328992/264712
        var ab = b - a;
        var cd = d - c;
        var radians = -MathF.Atan2(
            cd.X * ab.Y - cd.Y * ab.X,
            cd.X * ab.X + cd.Y * ab.Y);
        var abLength = ab.Length();
        var cdLength = cd.Length();
        var scale = cdLength / abLength;

        // {
        //     var step1 = a;
        //     var step2 = Vector2.Transform(step1, Matrix3x2.CreateTranslation(-a));
        //     var step3 = Vector2.Transform(step2, Matrix3x2.CreateRotation(radians));
        //     var step4 = Vector2.Transform(step3, Matrix3x2.CreateScale(scale));
        //     var step5 = Vector2.Transform(step4, Matrix3x2.CreateTranslation(c));
        //     var step6 = step5;
        // }

        // {
        //     var step1 = b;
        //     var step2 = Vector2.Transform(step1, Matrix3x2.CreateTranslation(-a));
        //     var step3 = Vector2.Transform(step2, Matrix3x2.CreateRotation(radians));
        //     var step4 = Vector2.Transform(step3, Matrix3x2.CreateScale(scale));
        //     var step5 = Vector2.Transform(step4, Matrix3x2.CreateTranslation(c));
        //     var step6 = step5;
        // }

        var result =
            Matrix3x2.CreateTranslation(-a) *
            Matrix3x2.CreateRotation(radians) *
            Matrix3x2.CreateScale(scale) *
            Matrix3x2.CreateTranslation(c);
        return result;
    }

    public static Matrix3x2 AlignX(
        Vector2 a,
        float bx,
        Vector2 c,
        float dx)
    {
        var abLength = bx - a.X;
        var cdLength = dx - c.X;
        var scale = cdLength / abLength;
        var result =
            Matrix3x2.CreateTranslation(-a) *
            Matrix3x2.CreateScale(scale) *
            Matrix3x2.CreateTranslation(c);
        return result;
    }

    public static Matrix3x2 AlignY(
        Vector2 a,
        float by,
        Vector2 c,
        float dy)
    {
        var abLength = by - a.Y;
        var cdLength = dy - c.Y;
        var scale = cdLength / abLength;
        var result =
            Matrix3x2.CreateTranslation(-a) *
            Matrix3x2.CreateScale(scale) *
            Matrix3x2.CreateTranslation(c);
        return result;
    }
}