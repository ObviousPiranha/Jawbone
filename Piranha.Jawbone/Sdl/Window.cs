using System;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sdl;

public class Window
{
    internal ISdl2 Sdl { get; }
    public IOpenGl OpenGl { get; }
    internal IWindowEventHandler WindowEventHandler { get; }
    internal IntPtr WindowPointer { get; }
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
        uint windowId)
    {
        Sdl = sdl;
        OpenGl = openGl;
        WindowEventHandler = windowEventHandler;
        WindowPointer = windowPointer;
        WindowId = windowId;
    }

    public void Close() => WasClosed = true;

    internal bool Loop(IntPtr contextPointer)
    {
        var stopwatch = ValueStopwatch.Start();
        Sdl.GlMakeCurrent(WindowPointer, contextPointer);
        var sdlMakeCurrent = stopwatch.GetElapsed();
        var sdlSwapWindow = TimeSpan.Zero;
        var result = WindowEventHandler.OnLoop(this);

        if (result)
        {
            stopwatch = ValueStopwatch.Start();
            Sdl.GlSwapWindow(WindowPointer);
            sdlSwapWindow = stopwatch.GetElapsed();
        }

        WindowEventHandler.ReportTimes(this, sdlMakeCurrent, sdlSwapWindow);
        return result;
    }

    internal bool Expose(IntPtr contextPointer)
    {
        var stopwatch = ValueStopwatch.Start();
        Sdl.GlMakeCurrent(WindowPointer, contextPointer);
        var sdlMakeCurrent = stopwatch.GetElapsed();
        var sdlSwapWindow = TimeSpan.Zero;
        var result = WindowEventHandler.OnExpose(this);

        if (result)
        {
            stopwatch = ValueStopwatch.Start();
            Sdl.GlSwapWindow(WindowPointer);
            sdlSwapWindow = stopwatch.GetElapsed();
        }

        WindowEventHandler.ReportTimes(this, sdlMakeCurrent, sdlSwapWindow);
        return result;
    }
}