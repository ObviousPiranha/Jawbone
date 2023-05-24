using System.Numerics;
using Piranha.Jawbone;
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
            .Append(new(position.A, textureCoordinates.A))
            .Append(new(position.B, textureCoordinates.B))
            .Append(new(position.C, textureCoordinates.C))
            .Append(new(position.A, textureCoordinates.A))
            .Append(new(position.C, textureCoordinates.C))
            .Append(new(position.D, textureCoordinates.D));
    }
}
