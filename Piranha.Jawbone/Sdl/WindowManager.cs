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

namespace Piranha.Jawbone.Sdl
{
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
        private readonly List<KeyValuePair<uint, IWindowEventHandler>> _activeWindows = new();
        private readonly Dictionary<uint, int> _exposeVersionId = new();

        public WindowManager(
            ISdl2 sdl,
            ILogger<WindowManager> logger)
        {
            _sdl = sdl;
            _logger = logger;

            var displayCount = _sdl.GetNumVideoDisplays();
            var word = displayCount == 1 ? "display" : "displays";
            _logger.LogDebug($"{displayCount} {word}");

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
        
        public uint AddWindow(
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
                _activeWindows.Add(KeyValuePair.Create(id, handler));
                _sdl.GetWindowSize(windowPtr, out var w, out var h);
                handler.OnOpen(id, w, h, _gl.Library);
                return id;
            }
            catch
            {
                _sdl.DestroyWindow(windowPtr);
                throw;
            }
        }

        public bool TryExpose(uint windowId)
        {
            var eventData = ArrayPool<byte>.Shared.Rent(64);

            try
            {
                _ = BitConverter.TryWriteBytes(eventData, SdlEvent.WindowEvent);
                _ = BitConverter.TryWriteBytes(eventData.AsSpan(4), _sdl.GetTicks());
                _ = BitConverter.TryWriteBytes(eventData.AsSpan(8), windowId);
                eventData[12] = SdlWindowEvent.Exposed;

                var result = _sdl.PushEvent(eventData);
                return result == 1;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(eventData, true);
            }
        }

        private void DestroyInactiveWindows()
        {
            int i = 0;

            while (i < _activeWindows.Count)
            {
                var pair = _activeWindows[i];
                
                if (pair.Value.Running)
                {
                    ++i;
                }
                else
                {
                    var ptr = _sdl.GetWindowFromID(pair.Key);

                    if (ptr.IsValid())
                    {
                        _sdl.DestroyWindow(ptr);
                    }
                    else
                    {
                        _logger.LogWarning("No window pointer found for window ID: {0}.", pair.Key);
                    }

                    _activeWindows.RemoveAt(i);
                }
            }
        }

        public void Run()
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

                foreach (var pair in _activeWindows)
                {
                    var evi = pair.Value.ExposeVersionId;
                    if (0 < evi)
                    {
                        if (!_exposeVersionId.TryGetValue(pair.Key, out var id) || evi != id)
                        {
                            _exposeVersionId[pair.Key] = evi;
                            doSleep = false;
                            Expose(pair.Key, pair.Value);
                        }
                    }
                }

                if (nextSecond <= Stopwatch.GetTimestamp())
                {
                    doSleep = false;

                    foreach (var pair in _activeWindows)
                        pair.Value.OnSecond();
                    
                    nextSecond += Stopwatch.Frequency;
                }

                DestroyInactiveWindows();

                if (doSleep)
                    Thread.Sleep(1);
            }

            // Flush the queue before exiting.
            while (_sdl.PollEvent(_eventData) == 1)
            {
                HandleEvent();
            }
        }

        private IWindowEventHandler? GetHandler(uint windowId)
        {
            foreach (var pair in _activeWindows)
            {
                if (pair.Key == windowId)
                    return pair.Value;
            }
            
            _logger.LogWarning("Unrecognized window ID: " + windowId);
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
                    GetHandler(view.WindowId)?.OnKeyDown(view);
                    break;
                }
                case SdlEvent.KeyUp:
                {
                    var view = new KeyboardEventView(_eventData);
                    GetHandler(view.WindowId)?.OnKeyUp(view);
                    break;
                }
                case SdlEvent.MouseMotion:
                {
                    var view = new MouseMotionEventView(_eventData);
                    GetHandler(view.WindowId)?.OnMouseMove(view);
                    break;
                }
                case SdlEvent.MouseWheel:
                {
                    var view = new MouseWheelEventView(_eventData);
                    GetHandler(view.WindowId)?.OnMouseWheel(view);
                    break; 
                }
                case SdlEvent.MouseButtonDown:
                {
                    var view = new MouseButtonEventView(_eventData);
                    GetHandler(view.WindowId)?.OnMouseButtonDown(view);
                    break;
                }
                case SdlEvent.MouseButtonUp:
                {
                    var view = new MouseButtonEventView(_eventData);
                    GetHandler(view.WindowId)?.OnMouseButtonUp(view);
                    break;
                }
                case SdlEvent.Quit:
                {
                    foreach (var pair in _activeWindows)
                        pair.Value.OnQuit();
                    
                    break;
                }
                default:
                {
                    _logger.LogTrace("event " + eventType);
                    break;
                }
            }
        }

        private void Expose(uint windowId, IWindowEventHandler handler)
        {
            var windowPtr = _sdl.GetWindowFromID(windowId);

            if (_gl is null)
            {
                _logger.LogWarning("GL interface is null. Skipping OnExpose.");
            }
            else if (windowPtr.IsValid())
            {
                _sdl.GlMakeCurrent(windowPtr, _contextPtr);
                handler.OnExpose(_gl.Library);
                _sdl.GlSwapWindow(windowPtr);
            }
            else
            {
                _logger.LogWarning($"Window {windowId} has no window pointer. Skipping OnExpose.");
            }
        }

        private void HandleWindowEvent()
        {
            var view = new WindowEventView(_eventData);
            var handler = GetHandler(view.WindowId);

            if (handler is not null)
            {
                switch (view.Event)
                {
                    case SdlWindowEvent.Shown: break;
                    case SdlWindowEvent.Hidden: break;
                    case SdlWindowEvent.Exposed: Expose(view.WindowId, handler); break; 
                    case SdlWindowEvent.Moved: handler.OnMove(view); break;
                    case SdlWindowEvent.Resized: handler.OnResize(view); break;
                    case SdlWindowEvent.SizeChanged: handler.OnSizeChanged(view); break;
                    case SdlWindowEvent.Minimized: handler.OnMinimize(); break;
                    case SdlWindowEvent.Maximized: handler.OnMaximize(); break;
                    case SdlWindowEvent.Restored: handler.OnRestore(); break;
                    case SdlWindowEvent.Enter: handler.OnMouseEnter(); break;
                    case SdlWindowEvent.Leave: handler.OnMouseLeave(); break;
                    case SdlWindowEvent.FocusGained: handler.OnInputFocus(); break;
                    case SdlWindowEvent.FocusLost: handler.OnInputBlur(); break;
                    case SdlWindowEvent.Close: handler.OnClose(); break;
                    default: break;
                }
            }
        }
    }
}
