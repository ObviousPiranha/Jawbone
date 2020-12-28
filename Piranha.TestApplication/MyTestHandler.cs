using System;
using System.IO;
using System.Numerics;
using System.Text;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;

namespace Piranha.TestApplication
{
    class MyTestHandler : IWindowEventHandler
    {
        private const uint Target = Gl.Texture2d;
        private static readonly Rectangle32 PiranhaSprite = new(
            new Point32(1, 1),
            new Point32(500, 250));

        private readonly float[] _bufferData = new float[24];
        private readonly IStb _stb;
        private readonly ISdl2 _sdl;
        private readonly ILogger<MyTestHandler> _logger;
        private readonly IWindowManager _windowManager;
        private readonly Random _random;
        private Matrix4x4 _matrix = default;
        private uint _windowId = 0;
        private int _width = 0;
        private int _height = 0;
        private uint _program = 0;
        private uint _texture = 0;
        private uint _buffer = default;
        private uint _vertexArray = default;
        private int _matrixUniform = default;
        private int _textureUniform = default;
        private int _positionAttribute = default;
        private int _textureCoordinateAttribute = default;

        private float Randumb() => (float)_random.NextDouble();

        public MyTestHandler(
            ILogger<MyTestHandler> logger,
            IWindowManager windowManager,
            IStb stb,
            ISdl2 sdl,
            Random random)
        {
            _stb = stb;
            _sdl = sdl;
            _logger = logger;
            _windowManager = windowManager;
            _random = random;
        }

        public bool Running { get; private set; }

        public void OnOpen(uint windowId, int width, int height, IOpenGl gl)
        {
            _windowId = windowId;
            _width = width;
            _height = height;
            Running = true;

            _ = GlTools.TryLogErrors(gl, _logger);

            _program = gl.CreateProgram();
            var vertexSource = File.ReadAllBytes("vertex.shader");
            var fragmentSource = File.ReadAllBytes("fragment.shader");
            var vertexShader = GlTools.LoadShader(gl, vertexSource, Gl.VertexShader);
            var fragmentShader = GlTools.LoadShader(gl, fragmentSource, Gl.FragmentShader);
            gl.AttachShader(_program, vertexShader);
            gl.AttachShader(_program, fragmentShader);
            gl.LinkProgram(_program);
            gl.GetProgramiv(_program, Gl.LinkStatus, out var result);
            if (result == Gl.False)
            {
                gl.GetProgramiv(_program, Gl.InfoLogLength, out var logLength);
                var buffer = new byte[logLength];
                gl.GetProgramInfoLog(_program, buffer.Length, out var actualLength, buffer);
                var errors = Encoding.UTF8.GetString(buffer);
                throw new OpenGlException("Error linking program: " + errors);
            }
            else
            {
                _logger.LogDebug("Linked shader program!");
            }

            gl.DeleteShader(fragmentShader);
            gl.DeleteShader(vertexShader);

            _ = GlTools.TryLogErrors(gl, _logger);

            _matrixUniform = gl.GetUniformLocation(_program, "theMatrix");
            _textureUniform = gl.GetUniformLocation(_program, "theTexture");
            _positionAttribute = gl.GetAttribLocation(_program, "position");
            _textureCoordinateAttribute = gl.GetAttribLocation(_program, "textureCoordinates");

            _ = GlTools.TryLogErrors(gl, _logger);

            _vertexArray = gl.GenVertexArray();
            gl.BindVertexArray(_vertexArray);

            _ = GlTools.TryLogErrors(gl, _logger);

            _texture = gl.GenTexture();
            gl.BindTexture(Target, _texture);

            _ = GlTools.TryLogErrors(gl, _logger);

            // Public domain piranha:
            // https://publicdomainvectors.org/en/free-clipart/Piranha-fish-vector/3815.html

            var pngBytes = File.ReadAllBytes("sheet.png");
            var pixelBytes = _stb.StbiLoadFromMemory(
                pngBytes[0],
                pngBytes.Length,
                out var w,
                out var h,
                out _,
                4);

            gl.TexImage2D(
                Target,
                0,
                Gl.Rgba8,
                w,
                h,
                0,
                Gl.Rgba,
                Gl.UnsignedByte,
                pixelBytes);
            
            _stb.StbiImageFree(pixelBytes);
            
            gl.TexParameteri(Target, Gl.TextureWrapS, Gl.ClampToEdge);
            gl.TexParameteri(Target, Gl.TextureWrapT, Gl.ClampToEdge);
            gl.TexParameteri(Target, Gl.TextureMagFilter, Gl.Nearest);
            gl.TexParameteri(Target, Gl.TextureMinFilter, Gl.Nearest);

            var positions = new Quadrilateral(new Vector2(-1F, 0.5F), new Vector2(1F, -0.5F));
            var textureCoordinates = PiranhaSprite.ToTextureCoordinates(new Point32(512, 512));
            var n = 0;
            _bufferData[n++] = positions.A.X;
            _bufferData[n++] = positions.A.Y;
            _bufferData[n++] = textureCoordinates.A.X;
            _bufferData[n++] = textureCoordinates.A.Y;

            _bufferData[n++] = positions.B.X;
            _bufferData[n++] = positions.B.Y;
            _bufferData[n++] = textureCoordinates.B.X;
            _bufferData[n++] = textureCoordinates.B.Y;

            _bufferData[n++] = positions.C.X;
            _bufferData[n++] = positions.C.Y;
            _bufferData[n++] = textureCoordinates.C.X;
            _bufferData[n++] = textureCoordinates.C.Y;

            _bufferData[n++] = positions.A.X;
            _bufferData[n++] = positions.A.Y;
            _bufferData[n++] = textureCoordinates.A.X;
            _bufferData[n++] = textureCoordinates.A.Y;

            _bufferData[n++] = positions.C.X;
            _bufferData[n++] = positions.C.Y;
            _bufferData[n++] = textureCoordinates.C.X;
            _bufferData[n++] = textureCoordinates.C.Y;

            _bufferData[n++] = positions.D.X;
            _bufferData[n++] = positions.D.Y;
            _bufferData[n++] = textureCoordinates.D.X;
            _bufferData[n++] = textureCoordinates.D.Y;

            var aspectRatio = width / (float)height;
            _matrix = Matrix4x4.CreateOrthographic(aspectRatio * 2f, 2f, 1f, -1f);

            _buffer = gl.GenBuffer();
            gl.BindBuffer(Gl.ArrayBuffer, _buffer);
            gl.BufferData(Gl.ArrayBuffer, new IntPtr(_bufferData.Length * 4), _bufferData[0], Gl.StaticDraw);

            _ = GlTools.TryLogErrors(gl, _logger);
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

            gl.UseProgram(_program);
            gl.Enable(Gl.Blend);
            gl.BlendFunc(Gl.SrcAlpha, Gl.OneMinusSrcAlpha);
            gl.BindVertexArray(_vertexArray);
            // gl.BindBuffer(Gl.ArrayBuffer, _buffer);
            gl.BindTexture(Gl.Texture2d, _texture);
            gl.EnableVertexAttribArray(_positionAttribute);
            gl.EnableVertexAttribArray(_textureCoordinateAttribute);
            gl.Uniform1i(_textureUniform, 0);
            gl.UniformMatrix4fv(_matrixUniform, 1, Gl.False, _matrix);
            gl.VertexAttribPointer(
                _positionAttribute,
                2,
                Gl.Float,
                Gl.False,
                16,
                IntPtr.Zero);
            gl.VertexAttribPointer(
                _textureCoordinateAttribute,
                2,
                Gl.Float,
                Gl.False,
                16,
                new IntPtr(2 * 4));
            gl.DrawArrays(Gl.Triangles, 0, 6);
            gl.DisableVertexAttribArray(_textureCoordinateAttribute);
            gl.DisableVertexAttribArray(_positionAttribute);
            gl.Disable(Gl.Blend);
            gl.UseProgram(0);
            _ = GlTools.TryLogErrors(gl, _logger);
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
            _ = _windowManager.TryExpose(_windowId);
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

            var aspectRatio = _width / (float)_height;
            _matrix = Matrix4x4.CreateOrthographic(aspectRatio * 2f, 2f, 1f, -1f);
        }

        public void OnRestore()
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
