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
            .Add(new(position.A, textureCoordinates.A))
            .Add(new(position.B, textureCoordinates.B))
            .Add(new(position.C, textureCoordinates.C))
            .Add(new(position.A, textureCoordinates.A))
            .Add(new(position.C, textureCoordinates.C))
            .Add(new(position.D, textureCoordinates.D));
    }
}
