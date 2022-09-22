using System;
using System.Numerics;
using Piranha.Jawbone.Collections;

namespace Piranha.SampleApplication
{
    class PiranhaScene
    {
        public Vector4 Color { get; set; }
        public UnmanagedList<Vertex> VertexData { get; } = new();
    }
}
