using Piranha.Jawbone.OpenGl;
using System;

namespace Piranha.Jawbone.Sdl;

public class GraphicsProvider
{
    private readonly ISdl2 _sdl;
    private readonly IntPtr _windowPointer;

    public IOpenGl Graphics { get; }

    public GraphicsProvider(
        ISdl2 sdl,
        IOpenGl gl,
        IntPtr windowPointer)
    {
        _sdl = sdl;
        Graphics = gl;
        _windowPointer = windowPointer;
    }

    public void Present()
    {
        _sdl.GlSwapWindow(_windowPointer);
    }
}
