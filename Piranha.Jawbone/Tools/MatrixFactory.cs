using System.Numerics;

namespace Piranha.Jawbone;

public static class MatrixFactory
{
    public static Matrix4x4 CreateSepia()
    {
        // https://stackoverflow.com/a/41994089/264712
        return new Matrix4x4(
            0.39f, 0.349f, 0.272f, 0f,
            0.769f, 0.686f, 0.534f, 0f,
            0.189f, 0.168f, 0.131f, 0f,
            0f, 0f, 0f, 1f);
    }

    public static Matrix4x4 CreateScaleW(float scale)
    {
        return new Matrix4x4(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, scale);
    }
}
