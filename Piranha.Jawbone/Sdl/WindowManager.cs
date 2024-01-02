using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Extensions;
using Piranha.Jawbone.OpenGl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

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

        logger.LogDebug($"Detected display of {mode.W}x{mode.H}.");

        if (Platform.IsRaspberryPi)
        {
            flags |= SdlWindow.FullScreen;
            pos = SdlWindowPos.Undefined;
            width = 1920;
            height = 1080;
        }
        else if (fullscreen)
        {
            pos = 0;
            width = mode.W;
            height = mode.H;
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
    private readonly ISdl2 _sdl;
    private readonly ILogger<WindowManager> _logger;
    private IOpenGl? _gl = default;
    private IntPtr _contextPtr = default;
    private SdlEvent _eventData;
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
        if (_contextPtr.IsValid())
            _sdl.GlDeleteContext(_contextPtr);

        if (_gl is not null)
        {
            _gl = null;
            _sdl.GlUnloadLibrary();
        }
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
                    if (_sdl.GlLoadLibrary() != 0)
                    {
                        throw new SdlException(
                            "Unable to load GL library: " + _sdl.GetError());
                    }
                    _gl = NativeLibraryInterface.CreateInterface<IOpenGl>(
                        "SdlOpenGl",
                        static methodName => "gl" + methodName,
                        _sdl.GlGetProcAddress);
                    var gl = _gl;

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
            var window = new Window(_sdl, _gl, handler, windowPtr, _contextPtr, id);
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
            while (_sdl.PollEvent(out _eventData) == 1)
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
        while (_sdl.PollEvent(out _eventData) == 1)
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
        switch (_eventData.Type)
        {
            case SdlEventType.TextInput:
            {
                ref var sdlEvent = ref _eventData.Text;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnTextInput(window, sdlEvent);
                break;
            }
            case SdlEventType.WindowEvent:
            {
                HandleWindowEvent();
                break;
            }
            case SdlEventType.KeyDown:
            {
                ref var sdlEvent = ref _eventData.Key;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnKeyDown(window, sdlEvent);
                break;
            }
            case SdlEventType.KeyUp:
            {
                ref var sdlEvent = ref _eventData.Key;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnKeyUp(window, sdlEvent);
                break;
            }
            case SdlEventType.MouseMotion:
            {
                ref var sdlEvent = ref _eventData.Motion;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseMove(window, sdlEvent);
                break;
            }
            case SdlEventType.MouseWheel:
            {
                ref var sdlEvent = ref _eventData.Wheel;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseWheel(window, sdlEvent);
                break;
            }
            case SdlEventType.MouseButtonDown:
            {
                ref var sdlEvent = ref _eventData.Button;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseButtonDown(window, sdlEvent);
                break;
            }
            case SdlEventType.MouseButtonUp:
            {
                ref var sdlEvent = ref _eventData.Button;
                var window = GetWindow(sdlEvent.WindowId);

                if (window is not null)
                    window.WindowEventHandler.OnMouseButtonUp(window, sdlEvent);
                break;
            }
            case SdlEventType.Quit:
            {
                foreach (var window in _activeWindows)
                    window.WindowEventHandler.OnQuit(window);

                break;
            }
            default:
            {
                _logger.LogTrace("event {eventType}", _eventData.Type);
                break;
            }
        }
    }

    private void HandleWindowEvent()
    {
        ref var sdlEvent = ref _eventData.Window;
        var window = GetWindow(sdlEvent.WindowId);

        if (window is not null)
        {
            var handler = window.WindowEventHandler;
            switch (sdlEvent.Event)
            {
                case SdlWindowEventType.Shown:
                    handler.OnShown(window);
                    break;
                case SdlWindowEventType.Hidden:
                    handler.OnHidden(window);
                    break;
                case SdlWindowEventType.Exposed:
                    handler.OnExpose(window);
                    break;
                case SdlWindowEventType.Moved:
                    handler.OnMove(window, sdlEvent);
                    break;
                case SdlWindowEventType.Resized:
                    handler.OnResize(window, sdlEvent);
                    break;
                case SdlWindowEventType.SizeChanged:
                    handler.OnSizeChanged(window, sdlEvent);
                    break;
                case SdlWindowEventType.Minimized:
                    handler.OnMinimize(window);
                    break;
                case SdlWindowEventType.Maximized:
                    handler.OnMaximize(window);
                    break;
                case SdlWindowEventType.Restored:
                    handler.OnRestore(window);
                    break;
                case SdlWindowEventType.Enter:
                    handler.OnMouseEnter(window);
                    break;
                case SdlWindowEventType.Leave:
                    handler.OnMouseLeave(window);
                    break;
                case SdlWindowEventType.FocusGained:
                    handler.OnInputFocus(window);
                    break;
                case SdlWindowEventType.FocusLost:
                    handler.OnInputBlur(window);
                    break;
                case SdlWindowEventType.Close:
                    handler.OnClose(window);
                    break;
                default:
                    break;
            }
        }
    }
}
