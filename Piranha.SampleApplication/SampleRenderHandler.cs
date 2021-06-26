using System;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;

namespace Piranha.SampleApplication
{
    public class SampleRenderHandler : IWindowEventHandler
    {
        private readonly ILogger<SampleRenderHandler> _logger;
        private readonly IStb _stb;
        private uint _program = default;
        private bool _wasInitialized = false;
        private Matrix4x4 _projectionMatrix = Matrix4x4.Identity;
        private int _width;
        private int _height;
        private uint _texture;
        private uint _buffer;
        private int _matrixUniform;
        private int _textureUniform;
        private int _positionAttribute;
        private int _colorAttribute;
        private int _textureCoordinateAttribute;
        private uint _vertexArray;

        private void InitializeGl(IOpenGl gl)
        {
            _program = GlTools.LoadShaderProgramFromFiles(
                gl,
                Platform.GetShaderPath("simple.vertex.shader"),
                Platform.GetShaderPath("simple.fragment.shader"));

            _ = GlTools.TryLogErrors(gl, _logger);

            _matrixUniform = gl.GetUniformLocation(_program, "theMatrix");
            _textureUniform = gl.GetUniformLocation(_program, "theTexture");
            _positionAttribute = gl.GetAttribLocation(_program, "position");
            _colorAttribute = gl.GetAttribLocation(_program, "lightColor");
            _textureCoordinateAttribute = gl.GetAttribLocation(_program, "textureCoordinates");
            _ = GlTools.TryLogErrors(gl, _logger);

            _vertexArray = gl.GenVertexArray();
            gl.BindVertexArray(_vertexArray);
            _ = GlTools.TryLogErrors(gl, _logger);

            var data = _stb.StbiLoad("../op.png", out var x, out var y, out var comp, 4);
            
            // gl.ActiveTexture(Gl.Texture0);
            _texture = gl.GenTexture();
            gl.BindTexture(Gl.Texture2d, _texture);

            _ = GlTools.TryLogErrors(gl, _logger);

            gl.TexImage2D(Gl.Texture2d, 0, Gl.Rgba, x, y, 0, Gl.Rgba, Gl.UnsignedByte, data);
            _stb.StbiImageFree(data);
            data = IntPtr.Zero;
            gl.TexParameteri(Gl.Texture2d, Gl.TextureWrapS, Gl.ClampToEdge);
            gl.TexParameteri(Gl.Texture2d, Gl.TextureWrapT, Gl.ClampToEdge);
            gl.TexParameteri(Gl.Texture2d, Gl.TextureMagFilter, Gl.Nearest);
            gl.TexParameteri(Gl.Texture2d, Gl.TextureMinFilter, Gl.Nearest);
            _ = GlTools.TryLogErrors(gl, _logger);

            var debugTriangle = new float[]
            {
                -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
                0.0f, 0.5f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f,
                0.5f, -0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f
            };

            _buffer = gl.GenBuffer();
            gl.BindBuffer(Gl.ArrayBuffer, _buffer);
            gl.BufferData(
                Gl.ArrayBuffer,
                new IntPtr(debugTriangle.Length * 4),
                debugTriangle[0],
                Gl.StaticDraw);

            _wasInitialized = true;
        }

        public SampleRenderHandler(
            ILogger<SampleRenderHandler> logger,
            IStb stb)
        {
            _logger = logger;
            _stb = stb;
        }

        public bool Running { get; private set; } = true;
        public int ExposeVersionId { get; private set; }

        public void RequestExpose() => ++ExposeVersionId;
        
        public void OnClose()
        {
            Running = false;
        }

        public void OnExpose(IOpenGl gl)
        {
            if (!_wasInitialized)
                InitializeGl(gl);
            else
                gl.BindVertexArray(_vertexArray);
            
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.Viewport(0, 0, _width, _height);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.UseProgram(_program);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.Enable(Gl.Blend);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.BlendFunc(Gl.SrcAlpha, Gl.OneMinusSrcAlpha);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.EnableVertexAttribArray(_positionAttribute);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.EnableVertexAttribArray(_colorAttribute);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.EnableVertexAttribArray(_textureCoordinateAttribute);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.ActiveTexture(Gl.Texture0);
            gl.Uniform1i(_textureUniform, 0);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.ClearColor(0, 0, 0.5f, 1);
            gl.Clear(Gl.ColorBufferBit);
            gl.BindTexture(Gl.Texture2d, _texture);
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.BindBuffer(Gl.ArrayBuffer, _buffer);
            _ = GlTools.TryLogErrors(gl, _logger);
            
            gl.UniformMatrix4fv(
                _matrixUniform,
                1,
                Gl.True,
                _projectionMatrix);
            const int Stride = 4 * 7;
            _ = GlTools.TryLogErrors(gl, _logger);

            gl.VertexAttribPointer(
                _positionAttribute,
                2,
                Gl.Float,
                Gl.False,
                Stride,
                IntPtr.Zero);
            
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.VertexAttribPointer(
                _textureCoordinateAttribute,
                2,
                Gl.Float,
                Gl.False,
                Stride,
                new IntPtr(8));

            _ = GlTools.TryLogErrors(gl, _logger);
            gl.VertexAttribPointer(
                _colorAttribute,
                3,
                Gl.Float,
                Gl.False,
                Stride,
                new IntPtr(16));
            _ = GlTools.TryLogErrors(gl, _logger);
            gl.DrawArrays(Gl.Triangles, 0, 3);
            _ = GlTools.TryLogErrors(gl, _logger);

            gl.DisableVertexAttribArray(_textureCoordinateAttribute);
            gl.DisableVertexAttribArray(_colorAttribute);
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

        public void OnResize(WindowEventView eventData)
        {
            UpdateSize(eventData.X, eventData.Y);
        }

        public void OnRestore()
        {
            
        }

        public void OnSecond()
        {
            
        }

        public void OnSizeChanged(WindowEventView eventData)
        {
            UpdateSize(eventData.X, eventData.Y);
        }

        public void OnOpen(uint windowId, int width, int height, IOpenGl gl)
        {
            UpdateSize(width, height);
        }

        private void UpdateSize(int width, int height)
        {
            _width = width;
            _height = height;

            var pps = 256f;
            var w = _width / pps;
            var h = _height / pps;
            _projectionMatrix = Matrix4x4.CreateOrthographic(w, h, 1f, -1f);
        }

        public bool OnUser(UserEventView eventdata) => false;

        public void OnQuit()
        {
            Running = false;
        }
    }
}
