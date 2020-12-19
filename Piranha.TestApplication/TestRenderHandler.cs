using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;

namespace Piranha.TestApplication
{
    public class TestRenderHandler : IWindowEventHandler
    {
        private IStb _stb;
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

        private uint LoadShader(
            IOpenGl gl,
            byte[] source,
            uint shaderType)
        {
            var shader = gl.CreateShader(shaderType);
            var ptr = Marshal.AllocHGlobal(source.Length);
            try
            {
                Marshal.Copy(source, 0, ptr, source.Length);
                gl.ShaderSource(shader, 1, ptr, source.Length);
                gl.CompileShader(shader);
                gl.GetShaderiv(shader, Gl.CompileStatus, out var result);
                if (result == Gl.False)
                {
                    gl.GetShaderiv(shader, Gl.InfoLogLength, out var logLength);
                    var buffer = new byte[logLength];
                    gl.GetShaderInfoLog(shader, buffer.Length, out var actualLength, buffer);
                    var errors = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine("Error compiling shader:");
                    Console.WriteLine(errors);
                }
                else
                {
                    Console.WriteLine("Compiled shader!");
                }

                return shader;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        private static void DumpErrors(
            IOpenGl gl,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? caller = null)
        {
            while (true)
            {
                var err = gl.GetError();

                if (err == Gl.NoError)
                    break;

                Console.Write($"{caller} : {lineNumber} - ");
                
                switch (err)
                {
                    case Gl.InvalidEnum: Console.WriteLine("GL_INVALID_ENUM"); break;
                    case Gl.InvalidValue: Console.WriteLine("GL_INVALID_VALUE"); break;
                    case Gl.InvalidOperation: Console.WriteLine("GL_INVALID_OPERATION"); break;
                    case Gl.StackOverflow: Console.WriteLine("GL_STACK_OVERFLOW"); break;
                    case Gl.StackUnderflow: Console.WriteLine("GL_STACK_UNDERFLOW"); break;
                    case Gl.OutOfMemory: Console.WriteLine("GL_OUT_OF_MEMORY"); break;
                    case Gl.InvalidFramebufferOperation: Console.WriteLine("GL_INVALID_FRAMEBUFFER_OPERATION"); break;
                    case Gl.ContextLost: Console.WriteLine("GL_CONTEXT_LOST"); break;
                    default: Console.WriteLine("Everything is fine. Nothing is broken."); break;
                }

                // Console.WriteLine(new System.Diagnostics.StackTrace());
            }
        }

        private void InitializeGl(IOpenGl gl)
        {
            _program = gl.CreateProgram();
            var vertexSource = File.ReadAllBytes(Platform.GetShaderPath("simple.vertex.shader"));
            var fragmentSource = File.ReadAllBytes(Platform.GetShaderPath("simple.fragment.shader"));
            var vertexShader = LoadShader(gl, vertexSource, Gl.VertexShader);
            var fragmentShader = LoadShader(gl, fragmentSource, Gl.FragmentShader);
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
                Console.WriteLine("Error linking program:");
                Console.WriteLine(errors);
            }
            else
            {
                Console.WriteLine("Linked program!");
            }

            gl.DeleteShader(fragmentShader);
            gl.DeleteShader(vertexShader);

            DumpErrors(gl);

            _matrixUniform = gl.GetUniformLocation(_program, "theMatrix");
            _textureUniform = gl.GetUniformLocation(_program, "theTexture");
            _positionAttribute = gl.GetAttribLocation(_program, "position");
            _colorAttribute = gl.GetAttribLocation(_program, "lightColor");
            _textureCoordinateAttribute = gl.GetAttribLocation(_program, "textureCoordinates");
            DumpErrors(gl);

            _vertexArray = gl.GenVertexArray();
            gl.BindVertexArray(_vertexArray);
            DumpErrors(gl);

            var data = _stb.StbiLoad("../op.png", out var x, out var y, out var comp, 4);
            
            // gl.ActiveTexture(Gl.Texture0);
            _texture = gl.GenTexture();
            gl.BindTexture(Gl.Texture2d, _texture);

            DumpErrors(gl);

            gl.TexImage2D(Gl.Texture2d, 0, Gl.Rgba, x, y, 0, Gl.Rgba, Gl.UnsignedByte, data);
            _stb.StbiImageFree(data);
            data = IntPtr.Zero;
            gl.TexParameteri(Gl.Texture2d, Gl.TextureWrapS, Gl.ClampToEdge);
            gl.TexParameteri(Gl.Texture2d, Gl.TextureWrapT, Gl.ClampToEdge);
            gl.TexParameteri(Gl.Texture2d, Gl.TextureMagFilter, Gl.Nearest);
            gl.TexParameteri(Gl.Texture2d, Gl.TextureMinFilter, Gl.Nearest);
            DumpErrors(gl);

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

        public TestRenderHandler(IStb stb)
        {
            _stb = stb;
        }

        public bool Running { get; private set; } = true;

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
            
            DumpErrors(gl);
            gl.Viewport(0, 0, _width, _height);
            DumpErrors(gl);
            gl.UseProgram(_program);
            DumpErrors(gl);
            gl.Enable(Gl.Blend);
            DumpErrors(gl);
            gl.BlendFunc(Gl.SrcAlpha, Gl.OneMinusSrcAlpha);
            DumpErrors(gl);
            gl.EnableVertexAttribArray(_positionAttribute);
            DumpErrors(gl);
            gl.EnableVertexAttribArray(_colorAttribute);
            DumpErrors(gl);
            gl.EnableVertexAttribArray(_textureCoordinateAttribute);
            DumpErrors(gl);
            gl.ActiveTexture(Gl.Texture0);
            gl.Uniform1i(_textureUniform, 0);
            DumpErrors(gl);
            gl.ClearColor(0, 0, 0.5f, 1);
            gl.Clear(Gl.ColorBufferBit);
            gl.BindTexture(Gl.Texture2d, _texture);
            DumpErrors(gl);
            gl.BindBuffer(Gl.ArrayBuffer, _buffer);
            DumpErrors(gl);
            
            gl.UniformMatrix4fv(
                _matrixUniform,
                1,
                Gl.True,
                _projectionMatrix);
            const int Stride = 4 * 7;
            DumpErrors(gl);

            gl.VertexAttribPointer(
                _positionAttribute,
                2,
                Gl.Float,
                Gl.False,
                Stride,
                IntPtr.Zero);
            
            DumpErrors(gl);
            gl.VertexAttribPointer(
                _textureCoordinateAttribute,
                2,
                Gl.Float,
                Gl.False,
                Stride,
                new IntPtr(8));

            DumpErrors(gl);
            gl.VertexAttribPointer(
                _colorAttribute,
                3,
                Gl.Float,
                Gl.False,
                Stride,
                new IntPtr(16));
            DumpErrors(gl);
            gl.DrawArrays(Gl.Triangles, 0, 3);
            DumpErrors(gl);

            gl.DisableVertexAttribArray(_textureCoordinateAttribute);
            gl.DisableVertexAttribArray(_colorAttribute);
            gl.DisableVertexAttribArray(_positionAttribute);
            gl.Disable(Gl.Blend);
            gl.UseProgram(0);
            DumpErrors(gl);
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

        public void OnOpen(uint windowId, int width, int height)
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
