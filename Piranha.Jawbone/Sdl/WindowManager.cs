using System;
using System.Buffers;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Tools;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Jawbone.Sdl
{
    public sealed class WindowManager : IWindowManager, IDisposable
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
        private readonly uint _customExposeEvent;
        private NativeLibraryInterface<IOpenGl>? _gl = default;
        private IntPtr _contextPtr = default;
        private readonly Dictionary<uint, IWindowEventHandler> _handlerByWindowId = new();
        private readonly List<KeyValuePair<IntPtr, IWindowEventHandler>> _activeWindows = new();

        public WindowManager(
            ISdl2 sdl,
            ILogger<WindowManager> logger)
        {
            _sdl = sdl;
            _logger = logger;

            var displayCount = _sdl.GetNumVideoDisplays();
            var word = displayCount == 1 ? "display" : "displays";
            _logger.LogDebug($"{displayCount} {word}");

            _customExposeEvent = _sdl.RegisterEvents(1);
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
            else
            {
                // _logger.LogDebug("configuring OpenGL 3.2");
                // _sdl.GlSetAttribute(SdlGl.ContextMajorVersion, 3);
                // _sdl.GlSetAttribute(SdlGl.ContextMinorVersion, 2);
                // _sdl.GlSetAttribute(SdlGl.ContextProfileMask, SdlGlContextProfile.Core);
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
                if (_contextPtr.IsInvalid())
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

                        var log = string.Concat(
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

                var id = GetWindowId(windowPtr);
                _handlerByWindowId.Add(id, handler);
                _activeWindows.Add(KeyValuePair.Create(windowPtr, handler));
                _sdl.GetWindowSize(windowPtr, out var w, out var h);
                handler.OnOpen(id, w, h);
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
                    _sdl.DestroyWindow(pair.Key);
                    _activeWindows.RemoveAt(i);
                }
            }
        }

        public void Run()
        {
            while (_activeWindows.Count > 0)
            {
                if (_sdl.WaitEvent(_eventData) == 1)
                {
                    HandleEvent();
                }
                else
                {
                    _logger.LogError(
                        "Failed to wait for an event: " +
                        _sdl.GetError());
                }

                DestroyInactiveWindows();
            }

            // Flush the queue before exiting.
            while (_sdl.PollEvent(_eventData) == 1)
            {
                HandleEvent();
            }
        }

        private IWindowEventHandler? GetHandler(uint windowId)
        {
            if (!_handlerByWindowId.TryGetValue(windowId, out var handler))
                _logger.LogWarning("Unrecognized window ID: " + windowId);
            
            return handler;
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
                    if (eventType == _customExposeEvent)
                    {
                        var view = new UserEventView(_eventData);
                        var handler = GetHandler(view.WindowId);

                        if (handler is not null)
                            Expose(view.WindowId, handler);
                    }
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
