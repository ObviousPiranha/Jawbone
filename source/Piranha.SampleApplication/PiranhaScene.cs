using Jawbone;
using System.Numerics;

namespace Piranha.SampleApplication;

class PiranhaScene
{
    public Vector4 Color { get; set; }
    public UnmanagedList<Vertex> VertexData { get; } = new();
}
