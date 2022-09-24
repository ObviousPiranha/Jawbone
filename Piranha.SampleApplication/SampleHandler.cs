using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Collections;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.SampleApplication
{
    class SampleHandler : IWindowEventHandler
    {
        private const uint Target = Gl.Texture2d;
        public static readonly Rectangle32 PiranhaSprite = new(
            new Point32(1, 1),
            new Point32(500, 250));

        private readonly UnmanagedList<Vertex> _bufferData = new();
        private readonly IStb _stb;
        private readonly ILogger<SampleHandler> _logger;
        private readonly IAudioManager _audioManager;
        private readonly ScenePool<PiranhaScene> _scenePool;
        private PiranhaScene _currentScene = new();
        private ShaderInputMapper _shaderInputMapper = default;
        private Matrix4x4 _matrix = default;
        private uint _program = default;
        private uint _texture = default;
        private uint _buffer = default;
        private uint _vertexArray = default;
        private int _matrixUniform = default;
        private int _textureUniform = default;

        public SampleHandler(
            ILogger<SampleHandler> logger,
            IAudioManager audioManager,
            IStb stb,
            ScenePool<PiranhaScene> scenePool)
        {
            _stb = stb;
            _logger = logger;
            _audioManager = audioManager;
            _scenePool = scenePool;
        }

        public void OnWindowCreated(Window window)
        {
            var graphicsProvider = window.GetGraphics();
            var gl = graphicsProvider.Graphics;
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
                gl.GetProgramInfoLog(_program, buffer.Length, out var actualLength, out buffer[0]);
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
            _shaderInputMapper = ShaderInputMapper.Create<Vertex>(gl, _program);

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
            gl.TexParameteri(Target, Gl.TextureMagFilter, Gl.Linear);
            gl.TexParameteri(Target, Gl.TextureMinFilter, Gl.Linear);

            var positions = Quadrilateral.Create(new Vector2(-1F, 0.5F), new Vector2(1F, -0.5F));
            var textureCoordinates = PiranhaSprite.ToTextureCoordinates(new Point32(512, 512));
            _bufferData.Clear().Add(positions, textureCoordinates);

            var size = window.Size;
            gl.Viewport(0, 0, size.X, size.Y);
            var aspectRatio = size.X / (float)size.Y;
            _matrix = Matrix4x4.CreateOrthographic(aspectRatio * 2f, 2f, 1f, -1f);

            _buffer = gl.GenBuffer();
            var bytes = _bufferData.Bytes;
            gl.BindBuffer(Gl.ArrayBuffer, _buffer);
            gl.BufferData(Gl.ArrayBuffer, new IntPtr(bytes.Length), bytes[0], Gl.StreamDraw);

            _ = GlTools.TryLogErrors(gl, _logger);

            // neck_snap-Vladimir-719669812.wav
            // Public domain audio: http://soundbible.com/1953-Neck-Snap.html
            var audioBytes = File.ReadAllBytes("crunch.ogg");
            int samples = _stb.StbVorbisDecodeMemory(
                audioBytes[0],
                audioBytes.Length,
                out var channelCount,
                out var sampleRate,
                out var output);
            
            if (output.IsInvalid())
                throw new Exception("Failed to load audio file.");
            
            try
            {
                _audioManager.PrepareAudio(
                    SdlAudio.S16Lsb,
                    sampleRate,
                    channelCount,
                    output.ToReadOnlySpan<byte>(samples * channelCount * Unsafe.SizeOf<short>()));
            }
            finally
            {
                _stb.PiranhaFree(output);
            }
        }
        
        public void OnClose(Window window)
        {
            _logger.LogDebug("OnClose");
            window.Close();
            _scenePool.Closed = true;
        }

        public void OnQuit(Window window)
        {
            _logger.LogDebug("OnQuit");
            window.Close();
        }

        public void OnLoop(Window window)
        {
            OnExpose(window);
        }

        public void OnExpose(Window window)
        {
            var graphicsProvider = window.GetGraphics();
            var gl = graphicsProvider.Graphics;
            gl.ClearColor(
                _currentScene.Color.X,
                _currentScene.Color.Y,
                _currentScene.Color.Z,
                _currentScene.Color.W);
            gl.Clear(Gl.ColorBufferBit);

            gl.UseProgram(_program);
            gl.Enable(Gl.Blend);
            gl.BlendFunc(Gl.SrcAlpha, Gl.OneMinusSrcAlpha);
            gl.BindVertexArray(_vertexArray);
            var latestScene = _scenePool.TakeLatestScene();

            if (latestScene is not null)
            {
                _scenePool.ReturnScene(_currentScene);
                _currentScene = latestScene;
                var bytes = _currentScene.VertexData.Bytes;
                
                gl.BufferSubData(
                    Gl.ArrayBuffer,
                    IntPtr.Zero,
                    new IntPtr(bytes.Length),
                    bytes[0]);
            }
            gl.BindTexture(Gl.Texture2d, _texture);
            _shaderInputMapper.Enable(gl);
            _shaderInputMapper.VertexAttribPointers(gl);
            gl.Uniform1i(_textureUniform, 0);
            gl.UniformMatrix4fv(_matrixUniform, 1, Gl.False, _matrix);
            gl.DrawArrays(Gl.Triangles, 0, 6);
            _shaderInputMapper.Disable(gl);
            gl.Disable(Gl.Blend);
            gl.UseProgram(0);
            _ = GlTools.TryLogErrors(gl, _logger);

            graphicsProvider.Present();
        }

        public void OnKeyUp(Window window, KeyboardEventView eventData)
        {
            if (eventData.PhysicalKeyCode == SdlScancode.Escape)
                window.Close();
        }

        public void OnMouseButtonDown(Window window, MouseButtonEventView eventData)
        {
            _audioManager.ScheduleLoopingAudio(0, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            // _audioManager.ScheduleAudio(0, default);
            // _audioManager.ScheduleAudio(0, TimeSpan.FromSeconds(0.2));
            // _audioManager.ScheduleAudio(0, TimeSpan.FromSeconds(0.4));
        }

        public void OnSizeChanged(Window window, WindowEventView eventData)
        {
            var graphicsProvider = window.GetGraphics();
            var gl = graphicsProvider.Graphics;
            gl.Viewport(0, 0, eventData.X, eventData.Y);
            var aspectRatio = eventData.X / (float)eventData.Y;
            _matrix = Matrix4x4.CreateOrthographic(aspectRatio * 2f, 2f, 1f, -1f);
        }
    }
}
