using Jawbone;
using System.Numerics;

namespace Piranha.SampleApplication3;

static class VertexExtensions
{
    public static UnmanagedList<Vertex> Add(
        this UnmanagedList<Vertex> list,
        Quad<Vector2> position,
        Quad<Vector2> textureCoordinates)
    {
        return list
            .Append(new(position.A, textureCoordinates.A))
            .Append(new(position.B, textureCoordinates.B))
            .Append(new(position.C, textureCoordinates.C))
            .Append(new(position.A, textureCoordinates.A))
            .Append(new(position.C, textureCoordinates.C))
            .Append(new(position.D, textureCoordinates.D));
    }
}
