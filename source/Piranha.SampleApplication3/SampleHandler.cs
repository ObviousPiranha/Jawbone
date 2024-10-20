using Microsoft.Extensions.Logging;
using Piranha.Jawbone;
using Piranha.Jawbone.Extensions;
using Piranha.Jawbone.OpenGl;
using Piranha.Jawbone.Sdl3;
using Piranha.Jawbone.Stb;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace Piranha.SampleApplication3;

class SampleHandler : ISdlEventHandler, IDisposable
{
    private const uint Target = Gl.Texture2d;
    public static readonly Rectangle32 PiranhaSprite = new(
        new Point32(1, 1),
        new Point32(500, 250));

    private readonly UnmanagedList<Vertex> _bufferData = new();
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
    private bool _isFullscreen = false;
    private int _eventCount = 0;
    private readonly nint _windowPtr;
    private readonly nint _contextPtr;
    private readonly OpenGlLibrary _gl;

    public bool Running { get; private set; } = true;

    public SampleHandler(
        ILogger<SampleHandler> logger,
        IAudioManager audioManager,
        ScenePool<PiranhaScene> scenePool)
    {
        _logger = logger;
        _audioManager = audioManager;
        _scenePool = scenePool;

        var cameraCount = Sdl.GetNumCameraDrivers();
        if (0 < cameraCount)
        {
            for (int i = 0; i < cameraCount; ++i)
                _logger.LogInformation("Camera: {name}", Sdl.GetCameraDriver(i));
        }
        else
        {
            _logger.LogInformation("No cameras found.");
        }

        _windowPtr = Sdl.CreateWindow(
            "Sample Application",
            1024,
            768,
            SdlWindowFlags.OpenGl | SdlWindowFlags.Resizable);

        if (_windowPtr.IsInvalid())
            SdlException.Throw("Unable to create window.");

        var context = OpenGlContext.Create(_windowPtr, _logger);
        _contextPtr = context.SdlGlContextPtr;
        _gl = context.OpenGl;

        OnWindowCreated();
    }

    public void OnWindowCreated()
    {
        _ = GlTools.TryLogErrors(_gl, _logger);

        _program = _gl.CreateProgram();
        var vertexSource = File.ReadAllBytes("vertex.shader");
        var fragmentSource = File.ReadAllBytes("fragment.shader");
        var vertexShader = GlTools.LoadShader(_gl, vertexSource, Gl.VertexShader);
        var fragmentShader = GlTools.LoadShader(_gl, fragmentSource, Gl.FragmentShader);
        _gl.AttachShader(_program, vertexShader);
        _gl.AttachShader(_program, fragmentShader);
        _gl.LinkProgram(_program);
        _gl.GetProgramiv(_program, Gl.LinkStatus, out var result);
        if (result == Gl.False)
        {
            _gl.GetProgramiv(_program, Gl.InfoLogLength, out var logLength);
            var buffer = new byte[logLength];
            _gl.GetProgramInfoLog(_program, buffer.Length, out var actualLength, out buffer[0]);
            var errors = Encoding.UTF8.GetString(buffer);
            throw new OpenGlException("Error linking program: " + errors);
        }
        else
        {
            _logger.LogDebug("Linked shader program!");
        }

        _gl.DeleteShader(fragmentShader);
        _gl.DeleteShader(vertexShader);

        _ = GlTools.TryLogErrors(_gl, _logger);

        _matrixUniform = _gl.GetUniformLocation(_program, "theMatrix");
        _textureUniform = _gl.GetUniformLocation(_program, "theTexture");
        _shaderInputMapper = ShaderInputMapper.Create<Vertex>(_gl, _program);

        _ = GlTools.TryLogErrors(_gl, _logger);

        _vertexArray = _gl.GenVertexArray();
        _gl.BindVertexArray(_vertexArray);

        _ = GlTools.TryLogErrors(_gl, _logger);

        _texture = _gl.GenTexture();
        _gl.BindTexture(Target, _texture);

        _ = GlTools.TryLogErrors(_gl, _logger);

        // Public domain piranha:
        // https://publicdomainvectors.org/en/free-clipart/Piranha-fish-vector/3815.html

        var pngBytes = File.ReadAllBytes("sheet.png");
        var pixelBytes = StbImage.LoadFromMemory(
            in pngBytes[0],
            pngBytes.Length,
            out var w,
            out var h,
            out _,
            4);
        var span = pixelBytes.ToReadOnlySpan<byte>(w * h * 4);

        _gl.TexImage2D(
            Target,
            0,
            Gl.Rgba8,
            w,
            h,
            0,
            Gl.Rgba,
            Gl.UnsignedByte,
            span[0]);

        StbImage.ImageFree(pixelBytes);

        _gl.TexParameteri(Target, Gl.TextureWrapS, Gl.ClampToEdge);
        _gl.TexParameteri(Target, Gl.TextureWrapT, Gl.ClampToEdge);
        _gl.TexParameteri(Target, Gl.TextureMagFilter, Gl.Linear);
        _gl.TexParameteri(Target, Gl.TextureMinFilter, Gl.Linear);

        var positions = Quad.Create(new Vector2(-1F, 0.5F), new Vector2(1F, -0.5F));
        var textureCoordinates = PiranhaSprite.ToTextureCoordinates(new Point32(512, 512));
        _bufferData.Clear();
        _bufferData.Add(positions, textureCoordinates);

        Point32 size;
        Sdl.GetWindowSize(_windowPtr, out size.X, out size.Y);
        _gl.Viewport(0, 0, size.X, size.Y);
        var aspectRatio = size.X / (float)size.Y;
        _matrix = Matrix4x4.CreateOrthographic(aspectRatio * 2f, 2f, 1f, -1f);

        _buffer = _gl.GenBuffer();
        var bytes = _bufferData.Bytes;
        _gl.BindBuffer(Gl.ArrayBuffer, _buffer);
        _gl.BufferData(Gl.ArrayBuffer, new IntPtr(bytes.Length), bytes[0], Gl.StreamDraw);

        _ = GlTools.TryLogErrors(_gl, _logger);

        // neck_snap-Vladimir-719669812.wav
        // Public domain audio: http://soundbible.com/1953-Neck-Snap.html
        var audioBytes = File.ReadAllBytes("crunch.ogg");
        int samples = StbVorbis.DecodeMemory(
            in audioBytes[0],
            audioBytes.Length,
            out var channelCount,
            out var sampleRate,
            out var output);

        if (output.IsInvalid())
            throw new Exception("Failed to load audio file.");

        try
        {
            var audioSpan = output.ToReadOnlySpan<short>(samples * channelCount);
            _audioManager.PrepareAudio(
                sampleRate,
                channelCount,
                audioSpan);
        }
        finally
        {
            C.Free(output);
        }
    }

    public void OnCommonEvent(SdlCommonEvent sdlEvent)
    {
        if (sdlEvent.Type != SdlEventType.MouseMotion)
            _logger.LogInformation("{eventType} [{id}]", sdlEvent.Type, ++_eventCount);
    }

    public void OnWindowCloseRequested(SdlWindowEvent sdlEvent)
    {
        // _logger.LogDebug("OnClose");
        Running = false;
        _scenePool.Closed = true;
    }

    public void OnQuit(SdlQuitEvent sdlEvent)
    {
        // _logger.LogDebug("OnQuit");
        Running = false;
    }

    public void OnLoop()
    {
        if (_scenePool.HasNewScene)
            Draw();
    }

    public void OnWindowExposed(SdlWindowEvent sdlEvent)
    {
        Draw();
    }

    public void Draw()
    {
        _gl.ClearColor(
            _currentScene.Color.X,
            _currentScene.Color.Y,
            _currentScene.Color.Z,
            _currentScene.Color.W);
        _gl.Clear(Gl.ColorBufferBit);

        _gl.UseProgram(_program);
        _gl.Enable(Gl.Blend);
        _gl.BlendFunc(Gl.SrcAlpha, Gl.OneMinusSrcAlpha);
        _gl.BindVertexArray(_vertexArray);
        var latestScene = _scenePool.TakeLatestScene();

        if (latestScene is not null)
        {
            _scenePool.ReturnScene(_currentScene);
            _currentScene = latestScene;
            var bytes = _currentScene.VertexData.Bytes;

            _gl.BufferSubData(
                Gl.ArrayBuffer,
                IntPtr.Zero,
                new IntPtr(bytes.Length),
                bytes[0]);
        }
        _gl.BindTexture(Gl.Texture2d, _texture);
        _shaderInputMapper.Enable(_gl);
        _shaderInputMapper.VertexAttribPointers(_gl);
        _gl.Uniform1i(_textureUniform, 0);
        _gl.UniformMatrix4fv(_matrixUniform, 1, Gl.False, _matrix);
        _gl.DrawArrays(Gl.Triangles, 0, 6);
        _shaderInputMapper.Disable(_gl);
        _gl.Disable(Gl.Blend);
        _gl.UseProgram(0);
        _ = GlTools.TryLogErrors(_gl, _logger);
        Sdl.GlSwapWindow(_windowPtr);
    }

    public void OnKeyUp(SdlKeyboardEvent eventData)
    {
        if (eventData.Scancode == SdlScancode.Escape)
            Running = false;
        else if (eventData.Scancode == SdlScancode.F11)
            Sdl.SetWindowFullscreen(_windowPtr, _isFullscreen = !_isFullscreen);
    }

    public void OnMouseButtonDown(SdlMouseButtonEvent eventData)
    {
        _audioManager.LoopAudio(0);
        // _audioManager.ScheduleLoopingAudio(0, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        // _audioManager.ScheduleAudio(0, default);
        // _audioManager.ScheduleAudio(0, TimeSpan.FromSeconds(0.2));
        // _audioManager.ScheduleAudio(0, TimeSpan.FromSeconds(0.4));
    }

    public void OnWindowPixelSizeChanged(SdlWindowEvent eventData)
    {
        _gl.Viewport(0, 0, eventData.Data1, eventData.Data2);
        var aspectRatio = eventData.Data1 / (float)eventData.Data2;
        _matrix = Matrix4x4.CreateOrthographic(aspectRatio * 2f, 2f, 1f, -1f);
    }

    public void Dispose()
    {
        Sdl.GlDestroyContext(_contextPtr);
        Sdl.DestroyWindow(_windowPtr);
    }

    public void OnCameraDeviceAdded(SdlCameraDeviceEvent sdlEvent)
    {
        _logger.LogInformation("Camera: {name}", Sdl.GetCameraName(sdlEvent.Which));
    }
}
