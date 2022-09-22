using System.Numerics;
using Piranha.Jawbone.Collections;
using Piranha.Jawbone.Tools;

namespace Piranha.SampleApplication;

static class VertexExtensions
{
    public static UnmanagedList<Vertex> Add(
        this UnmanagedList<Vertex> list,
        Quadrilateral<Vector2> position,
        Quadrilateral<Vector2> textureCoordinates)
    {
        return list
            .Add(new Vertex(position.A, textureCoordinates.A))
            .Add(new Vertex(position.B, textureCoordinates.B))
            .Add(new Vertex(position.C, textureCoordinates.C))
            .Add(new Vertex(position.A, textureCoordinates.A))
            .Add(new Vertex(position.C, textureCoordinates.C))
            .Add(new Vertex(position.D, textureCoordinates.D));
    }
}
