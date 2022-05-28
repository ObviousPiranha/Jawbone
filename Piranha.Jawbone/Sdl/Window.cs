using System;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sdl;

public class Window
{
    private readonly GraphicsProvider _graphicsProvider;

    internal ISdl2 Sdl { get; }
    internal IWindowEventHandler WindowEventHandler { get; }
    internal IntPtr WindowPointer { get; }
    internal IntPtr ContextPointer { get; }
    internal uint WindowId { get; }

    public bool WasClosed { get; private set; }

    public Point32 Size
    {
        get
        {
            Sdl.GetWindowSize(WindowPointer, out var w, out var h);
            return new Point32(w, h);
        }

        set
        {
            Sdl.SetWindowSize(WindowPointer, value.X, value.Y);
        }
    }

    internal Window(
        ISdl2 sdl,
        IOpenGl openGl,
        IWindowEventHandler windowEventHandler,
        IntPtr windowPointer,
        IntPtr contextPointer,
        uint windowId)
    {
        Sdl = sdl;
        WindowEventHandler = windowEventHandler;
        WindowPointer = windowPointer;
        ContextPointer = contextPointer;
        WindowId = windowId;

        _graphicsProvider = new GraphicsProvider(sdl, openGl, windowPointer);
    }

    public void Close() => WasClosed = true;

    public GraphicsProvider GetGraphics()
    {
        Sdl.GlMakeCurrent(WindowPointer, ContextPointer);
        return _graphicsProvider;
    }
}
