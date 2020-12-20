using System;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Sdl;

namespace Piranha.TestApplication
{
    class MyTestHandler : IWindowEventHandler
    {
        private readonly ILogger<MyTestHandler> _logger;
        private readonly Random _random;
        private int _width = 0;
        private int _height = 0;

        private float Randumb() => (float)_random.NextDouble();

        public MyTestHandler(ILogger<MyTestHandler> logger, Random random)
        {
            _logger = logger;
            _random = random;
        }

        public bool Running { get; private set; }

        public void OnOpen(uint windowId, int width, int height)
        {
            _width = width;
            _height = height;
            Running = true;
        }
        
        public void OnClose()
        {
            _logger.LogDebug("OnClose");
            Running = false;
        }

        public void OnQuit()
        {
            _logger.LogDebug("OnQuit");
            Running = false;
        }

        public void OnExpose(IOpenGl gl)
        {
            gl.Viewport(0, 0, _width, _height);
            gl.ClearColor(Randumb(), Randumb(), Randumb(), 1.0F);
            gl.Clear(Gl.ColorBufferBit);
        }

        public void OnInputBlur()
        {
        }

        public void OnInputFocus()
        {
        }

        public void OnKeyDown(KeyboardEventView eventData)
        {
        }

        public void OnKeyUp(KeyboardEventView eventData)
        {
            _logger.LogDebug("OnKeyUp");
            if (eventData.PhysicalKeyCode == SdlScancode.Escape)
                Running = false;
        }

        public void OnMaximize()
        {
        }

        public void OnMinimize()
        {
        }

        public void OnMouseButtonDown(MouseButtonEventView eventData)
        {
        }

        public void OnMouseButtonUp(MouseButtonEventView eventData)
        {
        }

        public void OnMouseEnter()
        {
        }

        public void OnMouseLeave()
        {
        }

        public void OnMouseMove(MouseMotionEventView eventData)
        {
        }

        public void OnMouseWheel(MouseWheelEventView eventData)
        {
        }

        public void OnMove(WindowEventView eventData)
        {
        }

        public void OnPrepareRender()
        {
        }

        public void OnRender()
        {
        }

        public void OnResize(WindowEventView eventData)
        {
            _width = eventData.X;
            _height = eventData.Y;
        }

        public void OnRestore()
        {
        }

        public void OnSecond()
        {
        }

        public void OnSizeChanged(WindowEventView eventData)
        {
            _width = eventData.X;
            _height = eventData.Y;
        }

        public void OnUpdate()
        {
        }

        public bool OnUser(UserEventView eventdata) => false;
    }
}
