using System.Numerics;

namespace Piranha.SampleApplication;

struct Vertex
{
    public Vector2 Position;
    public Vector2 TextureCoordinates;

    public Vertex(Vector2 position, Vector2 textureCoordinates)
    {
        Position = position;
        TextureCoordinates = textureCoordinates;
    }
}
