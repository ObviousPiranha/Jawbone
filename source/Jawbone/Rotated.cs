using System.Numerics;

namespace Jawbone;

public static class Rotated
{
    public static Vector2 Clockwise(Vector2 vector) => new(vector.Y, -vector.X);
    public static Vector2 Counterclockwise(Vector2 vector) => new(-vector.Y, vector.X);
}
