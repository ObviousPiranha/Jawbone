using Piranha.Jawbone;
using System.Numerics;

namespace Piranha.SampleApplication3;

class PiranhaScene
{
    public Vector4 Color { get; set; }
    public UnmanagedList<Vertex> VertexData { get; } = new();
}
