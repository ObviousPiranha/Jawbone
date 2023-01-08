using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Tools;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Jawbone.Sdl;

sealed class WindowManager : IWindowManager, IDisposable
{
    private static IntPtr CreateWindowPtr(
        ISdl2 sdl,
        ILogger logger,
        string title,
        int width,
        int height,
        bool fullscreen)
    {
        var flags = SdlWindow.OpenGl;
        var pos = SdlWindowPos.Centered;

        var result = sdl.GetDesktopDisplayMode(0, out var mode);
        if (result < 0)
            throw new SdlException("Unable to get desktop display mode: " + sdl.GetError());
        
        logger.LogDebug($"Detected display of {mode.w}x{mode.h}.");

        if (fullscreen)
        {
            pos = 0;
            width = mode.w;
            height = mode.h;
        }
        else
        {
            flags |= SdlWindow.Resizable | SdlWindow.Shown;
        }
        
        var windowPtr = sdl.CreateWindow(
            title,
            pos,
            pos,
            width,
            height,
            flags);
        
        if (windowPtr.IsInvalid())
            throw new SdlException($"Unable to create window ({windowPtr}): {sdl.GetError()}");

        return windowPtr;
    }

    private readonly byte[] _eventData = new byte[56];
    private readonly ISdl2 _sdl;
    private readonly ILogger<WindowManager> _logger;
    private NativeLibraryInterface<IOpenGl>? _gl = default;
    private IntPtr _contextPtr = default;
    private readonly List<Window> _activeWindows = new();

    public WindowManager(
        ISdl2 sdl,
        ILogger<WindowManager> logger)
    {
        _sdl = sdl;
        _logger = logger;

        var displayCount = _sdl.GetNumVideoDisplays();
        var word = displayCount == 1 ? "display" : "displays";
        _logger.LogDebug($"{displayCount} {word}");
    }

    public void Dispose()
    {
        _gl?.Dispose();

        if (_contextPtr.IsValid())
            _sdl.GlDeleteContext(_contextPtr);
    }

    private uint GetWindowId(IntPtr windowPtr)
    {
        var windowId = _sdl.GetWindowID(windowPtr);

        if (windowId == 0)
            throw new SdlException("No window ID for " + windowPtr);
        
        return windowId;
    }
    
    public void AddWindow(
        string title,
        int width,
        int height,
        bool fullscreen,
        IWindowEventHandler handler)
    {
        var windowPtr = CreateWindowPtr(_sdl, _logger, title, width, height, fullscreen);

        try
        {
            if (_gl is null)
            {
                _sdl.GlSetAttribute(SdlGl.RedSize, 8);
                _sdl.GlSetAttribute(SdlGl.GreenSize, 8);
                _sdl.GlSetAttribute(SdlGl.BlueSize, 8);
                _sdl.GlSetAttribute(SdlGl.AlphaSize, 8);
                // _sdl.GlSetAttribute(SdlGl.DepthSize, 24);
                _sdl.GlSetAttribute(SdlGl.DoubleBuffer, 1);

                if (Platform.IsRaspberryPi)
                {
                    _logger.LogDebug("configuring OpenGL ES 3.0");
                    _sdl.GlSetAttribute(SdlGl.ContextMajorVersion, 3);
                    _sdl.GlSetAttribute(SdlGl.ContextMinorVersion, 0);
                    _sdl.GlSetAttribute(SdlGl.ContextProfileMask, SdlGlContextProfile.Es);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    _logger.LogDebug("configuring OpenGL 3.2");
                    _sdl.GlSetAttribute(SdlGl.ContextMajorVersion, 3);
                    _sdl.GlSetAttribute(SdlGl.ContextMinorVersion, 2);
                    _sdl.GlSetAttribute(SdlGl.ContextProfileMask, SdlGlContextProfile.Core);
                }

                _contextPtr = _sdl.GlCreateContext(windowPtr);

                if (_contextPtr.IsInvalid())
                {
                    throw new SdlException(
                        "Unable to create GL context: " + _sdl.GetError());
                }

                try
                {
                    _gl = OpenGlLoader.Load();
                    var gl = _gl.Library;

                    gl.GetIntegerv(Gl.MaxTextureSize, out var maxTextureSize);
                    
                    var version = new byte[4];
                    _sdl.GetVersion(out version[0]);

                    var log = string.Concat(
                        "SDL version: ",
                        string.Join('.', version),
                        Environment.NewLine,
                        "OpenGL version: ",
                        gl.GetString(Gl.Version),
                        Environment.NewLine,
                        "OpenGL shading language version: ",
                        gl.GetString(Gl.ShadingLanguageVersion),
                        Environment.NewLine,
                        "OpenGL vendor: ",
                        gl.GetString(Gl.Vendor),
                        Environment.NewLine,
                        "OpenGL renderer: ",
                        gl.GetString(Gl.Renderer),
                        Environment.NewLine,
                        "OpenGL max texture size: ",
                        maxTextureSize);
                    
                    _logger.LogDebug(log);
                }
                catch
                {
                    _sdl.GlDeleteContext(_contextPtr);
                    _contextPtr = default;
                    throw;
                }
            }
            else
            {
                _sdl.GlMakeCurrent(windowPtr, _contextPtr);
            }

            var id = GetWindowId(windowPtr);
            var window = new Window(_sdl, _gl.Library, handler, windowPtr, _contextPtr, id);
            _activeWindows.Add(window);
            handler.OnWindowCreated(window);
        }
        catch
        {
            _sdl.DestroyWindow(windowPtr);
            throw;
        }
    }

    private void DestroyInactiveWindows()
    {
        int i = 0;

        while (i < _activeWindows.Count)
        {
            var window = _activeWindows[i];
            
            if (window.WasClosed)
            {
                window.WindowEventHandler.OnDestroyingWindow(window);
                _sdl.DestroyWindow(window.WindowPointer);
                _activeWindows.RemoveAt(i);
            }
            else
            {
                ++i;
            }
        }
    }

    public void Run(IWindowManagerMetricHandler? windowManagerMetricHandler = null)
    {
        var nextSecond = Stopwatch.GetTimestamp() + Stopwatch.Frequency;

        while (_activeWindows.Count > 0)
        {
            var doSleep = true;
            while (_sdl.PollEvent(_eventData) == 1)
            {
                HandleEvent();
                doSleep = false;
            }

            foreach (var window in _activeWindows)
            {
                window.WindowEventHandler.OnLoop(window);
            }

            if (nextSecond <= Stopwatch.GetTimestamp())
            {
                doSleep = false;

                foreach (var window in _activeWindows)
                    window.WindowEventHandler.OnSecond(window);
                
                nextSecond += Stopwatch.Frequency;
            }

            DestroyInactiveWindows();

            if (doSleep)
            {
                var stopwatch = ValueStopwatch.Start();
                Thread.Sleep(1);
                windowManagerMetricHandler?.ReportSleepTime(stopwatch.GetElapsed());
            }
        }

        // Flush the queue before exiting.
        while (_sdl.PollEvent(_eventData) == 1)
        {
            HandleEvent();
        }
    }

    private Window? GetWindow(uint windowId)
    {
        foreach (var window in _activeWindows)
        {
            if (window.WindowId == windowId)
                return window;
        }

        _logger.LogWarning("Unrecognized window ID: {windowId}", windowId);
        return null;
    }

    private void HandleEvent()
    {
        var eventType = BitConverter.ToUInt32(_eventData, 0);

        switch (eventType)
        {
            case SdlEvent.WindowEvent:
            {
                HandleWindowEvent();
                break;
            }
            case SdlEvent.KeyDown:
            {
                var view = new KeyboardEventView(_eventData);
                var window = GetWindow(view.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnKeyDown(window, view);
                break;
            }
            case SdlEvent.KeyUp:
            {
                var view = new KeyboardEventView(_eventData);
                var window = GetWindow(view.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnKeyUp(window, view);
                break;
            }
            case SdlEvent.MouseMotion:
            {
                var view = new MouseMotionEventView(_eventData);
                var window = GetWindow(view.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseMove(window, view);
                break;
            }
            case SdlEvent.MouseWheel:
            {
                var view = new MouseWheelEventView(_eventData);
                var window = GetWindow(view.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseWheel(window, view);
                break; 
            }
            case SdlEvent.MouseButtonDown:
            {
                var view = new MouseButtonEventView(_eventData);
                var window = GetWindow(view.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseButtonDown(window, view);
                break;
            }
            case SdlEvent.MouseButtonUp:
            {
                var view = new MouseButtonEventView(_eventData);
                var window = GetWindow(view.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseButtonUp(window, view);
                break;
            }
            case SdlEvent.Quit:
            {
                foreach (var window in _activeWindows)
                    window.WindowEventHandler.OnQuit(window);
                
                break;
            }
            default:
            {
                _logger.LogTrace("event {eventType}", eventType);
                break;
            }
        }
    }

    private void HandleWindowEvent()
    {
        var view = new WindowEventView(_eventData);
        var window = GetWindow(view.WindowId);

        if (window is not null)
        {
            var handler = window.WindowEventHandler;
            switch (view.Event)
            {
                case SdlWindowEvent.Shown: handler.OnShown(window); break;
                case SdlWindowEvent.Hidden: handler.OnHidden(window); break;
                case SdlWindowEvent.Exposed: handler.OnExpose(window); break; 
                case SdlWindowEvent.Moved: handler.OnMove(window, view); break;
                case SdlWindowEvent.Resized: handler.OnResize(window, view); break;
                case SdlWindowEvent.SizeChanged: handler.OnSizeChanged(window, view); break;
                case SdlWindowEvent.Minimized: handler.OnMinimize(window); break;
                case SdlWindowEvent.Maximized: handler.OnMaximize(window); break;
                case SdlWindowEvent.Restored: handler.OnRestore(window); break;
                case SdlWindowEvent.Enter: handler.OnMouseEnter(window); break;
                case SdlWindowEvent.Leave: handler.OnMouseLeave(window); break;
                case SdlWindowEvent.FocusGained: handler.OnInputFocus(window); break;
                case SdlWindowEvent.FocusLost: handler.OnInputBlur(window); break;
                case SdlWindowEvent.Close: handler.OnClose(window); break;
                default: break;
            }
        }
    }
}
