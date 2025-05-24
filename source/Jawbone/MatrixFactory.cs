using System.Numerics;

namespace Jawbone;

public static class MatrixFactory
{
    public static Matrix4x4 CreateUnitOrthographic(float aspectRatio)
    {
        var result = aspectRatio < 1f ?
            Matrix4x4.CreateOrthographic(1f, 1f / aspectRatio, 1f, -1f) :
            Matrix4x4.CreateOrthographic(aspectRatio, 1f, 1f, -1f);

        return result;
    }

    public static Matrix4x4 CreateSepia()
    {
        // https://stackoverflow.com/a/41994089/264712
        return new Matrix4x4(
            0.39f, 0.349f, 0.272f, 0f,
            0.769f, 0.686f, 0.534f, 0f,
            0.189f, 0.168f, 0.131f, 0f,
            0f, 0f, 0f, 1f);
    }

    public static Matrix4x4 CreateScale(
        float x = 1f,
        float y = 1f,
        float z = 1f,
        float w = 1f)
    {
        return new Matrix4x4(
            x, 0f, 0f, 0f,
            0f, y, 0f, 0f,
            0f, 0f, z, 0f,
            0f, 0f, 0f, w);
    }

    public static Matrix4x4 CreateGrayscale()
    {
        const float A = 0.299f;
        const float B = 0.587f;
        const float C = 0.114f;

        // https://stackoverflow.com/a/2757025/264712
        return new Matrix4x4(
            A, A, A, 0f,
            B, B, B, 0f,
            C, C, C, 0f,
            0f, 0f, 0f, 1f);
    }
}
