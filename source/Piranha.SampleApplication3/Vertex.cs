using Piranha.Jawbone.OpenGl;
using System.Numerics;

namespace Piranha.SampleApplication3;

struct Vertex
{
    [ShaderInput("position")]
    public Vector2 Position;

    [ShaderInput("textureCoordinates")]
    public Vector2 TextureCoordinates;

    public Vertex(Vector2 position, Vector2 textureCoordinates)
    {
        Position = position;
        TextureCoordinates = textureCoordinates;
    }
}
