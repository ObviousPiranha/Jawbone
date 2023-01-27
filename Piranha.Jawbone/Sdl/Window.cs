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

    public int DisplayIndex => Sdl.GetWindowDisplayIndex(WindowPointer);

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

    public void ToggleFullScreen()
    {
        // https://superuser.com/a/1251294
        // http://lists.libsdl.org/pipermail/commits-libsdl.org/2018-January/002542.html
        // https://discourse.libsdl.org/t/cannot-remove-the-window-title-bar-and-borders/25615
        // https://discourse.libsdl.org/t/true-borderless-fullscreen-behaviour/24622

        int displayIndex = Sdl.GetWindowDisplayIndex(WindowPointer);
        if (displayIndex < 0)
            throw new SdlException("error getting window display");

        if (Sdl.GetDisplayUsableBounds(displayIndex, out var rect) != 0)
            throw new SdlException("error getting usable bounds");

        var flag = Sdl.GetWindowFlags(WindowPointer) & SdlWindow.FullScreenDesktop;
        Sdl.SetWindowFullscreen(WindowPointer, flag ^ SdlWindow.FullScreenDesktop);
    }
}
