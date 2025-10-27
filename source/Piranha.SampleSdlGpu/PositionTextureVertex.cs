using System.Net.Sockets;
using System.Numerics;

namespace Jawbone.SampleSdlGpu;

struct PositionTextureVertex
{
    public Vector3 Position;
    public Vector2 TextureCoordinates;

    public PositionTextureVertex(
        float x,
        float y,
        float z,
        float u,
        float v)
    {
        Position = new(x, y, z);
        TextureCoordinates = new(u, v);
    }
}