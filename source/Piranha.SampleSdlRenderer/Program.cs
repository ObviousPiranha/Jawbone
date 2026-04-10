using Jawbone;
using Jawbone.Sdl3;
using System;
using System.Diagnostics;
using System.Numerics;

namespace Piranha.SampleSdlRenderer;

// https://examples.libsdl.org/SDL3/renderer/08-rotating-textures/
class Program : ISdlEventHandler
{
    private nint _window;
    private nint _renderer;
    private nint _texture;
    private Point32 _imageSize;
    private int _width = 640;
    private int _height = 480;

    private readonly long _frameLength;
    private long _nextFrame;
    private int _rotateTick;

    public bool Running { get; private set; } = true;

    public Program()
    {
        _frameLength = Stopwatch.Frequency / 60;
        _nextFrame = Stopwatch.GetTimestamp();
    }

    public void OnStart()
    {
        Sdl.SetHint("SDL_RENDER_DRIVER", "direct3d12,metal,vulkan,opengl");
        Sdl.CreateWindowAndRenderer("Hello SDL3", _width, _height, SdlWindowFlags.Resizable, out _window, out _renderer);
        Sdl.SetRenderLogicalPresentation(_renderer, _width, _height, SdlRendererLogicalPresentation.Letterbox)
            .ThrowOnSdlFailure("Error on SDL_SetRenderLogicalPresentation.");
        Console.WriteLine("So far so good.");
        var surface = Sdl.LoadPng("kenney_iconCross_blue.png\0"u8[0])
            .ThrowOnSdlFailure("Failed to load PNG.");
        _imageSize = SdlExtensions.GetSurfaceSize(surface);
        Console.WriteLine("Image size: " + _imageSize);
        _texture = Sdl.CreateTextureFromSurface(_renderer, surface)
            .ThrowOnSdlFailure("Unable to create texture.");
        Sdl.DestroySurface(surface);

        {
            var (major, minor, micro) = SdlExtensions.GetVersion();
            Console.WriteLine(
                $"SDL version: {major}.{minor}.{micro}");
            var gpuDrivers = string.Join(", ", SdlExtensions.EnumerateGpuDrivers());
            Console.WriteLine($"Available GPU drivers: {gpuDrivers}");
            Console.WriteLine($"SDL video driver: {Sdl.GetCurrentVideoDriver()}");

            var rendererName = Sdl.GetRendererName(_renderer);
            Console.WriteLine("Renderer Name: " + rendererName);
        }
    }

    public void OnStop()
    {
        Sdl.DestroyTexture(_texture);
        Sdl.DestroyRenderer(_renderer);
        Sdl.DestroyWindow(_window);
    }

    public void OnQuit(SdlQuitEvent sdlEvent)
    {
        Running = false;
    }

    public void OnWindowCloseRequested(SdlWindowEvent sdlEvent)
    {
        Running = false;
    }

    public void OnWindowResized(SdlWindowEvent sdlEvent)
    {
        Console.WriteLine($"OnWindowResized: {sdlEvent.X}x{sdlEvent.Y}");
        ChangeSize(sdlEvent.X, sdlEvent.Y);
    }

    public void OnWindowPixelSizeChanged(SdlWindowEvent sdlEvent)
    {
        Console.WriteLine($"OnWindowPixelSizeChanged: {sdlEvent.X}x{sdlEvent.Y}");
        ChangeSize(sdlEvent.X, sdlEvent.Y);
    }

    private void ChangeSize(int w, int h)
    {
        if (w == _width || h == _height)
            return;
        // _width = w;
        // _height = h;
    }

    public void OnKeyDown(SdlKeyboardEvent sdlEvent)
    {
        switch (sdlEvent.Scancode)
        {
            case SdlScancode.Escape:
                Running = false;
                break;
            case SdlScancode.F11:
                SdlExtensions
                    .ToggleFullscreen(_window)
                    .ThrowOnSdlFailure("Unable to toggle fullscreen.");
                break;
        }
    }

    public void OnLoop()
    {
        var now = Stopwatch.GetTimestamp();
        if (_nextFrame <= now)
        {
            do
            {
                Step();
                _nextFrame += _frameLength;
            }
            while (_nextFrame <= now);

            Render();
        }
    }

    private void Step()
    {
        _rotateTick = (_rotateTick + 1) % 240;
    }

    private void Render()
    {
        Sdl.SetRenderDrawColor(_renderer, 0, 0, 0, 255);
        Sdl.RenderClear(_renderer);
        var srcRect = new SdlFRect
        {
            W = _imageSize.X / 2f,
            H = _imageSize.Y / 2f
        };
        var dstRect = new SdlFRect
        {
            X = (_width - _imageSize.X) / 2f,
            Y = (_height - _imageSize.Y) / 2f,
            W = _imageSize.X,
            H = _imageSize.Y
        };
        var center = new SdlFPoint
        {
            X = _imageSize.X / 2f,
            Y = _imageSize.Y / 2f
        };
        var angle = _rotateTick * 360 / 240d;
        Sdl.SetRenderScale(_renderer, 1f, 1f);
        // Sdl.RenderTextureRotated(_renderer, _texture, Unsafe.NullRef<SdlFRect>(), dstRect, angle, center, SdlFlipMode.None);
        Sdl.RenderTextureRotated(_renderer, _texture, srcRect, dstRect, angle, center, SdlFlipMode.None);

        var p = new Vector2(_width, _height) / 4f;
        Sdl.SetRenderDrawColor(_renderer, 255, 0, 0, 255);
        Sdl.SetRenderScale(_renderer, 2f, 2f);
        Sdl.RenderDebugText(_renderer, p.X, p.Y, "Hello, Friend");
        
        Sdl.SetRenderDrawColor(_renderer, 0, 0, 255, 255);
        Sdl.SetRenderScale(_renderer, 1f, 1f);
        Sdl.RenderDebugText(_renderer, p.X, p.Y, "Hello, Friend");
        Sdl.RenderPresent(_renderer);
    }

    private static int Main(string[] args)
    {
        try
        {
            // return SdlExtensions.RunApp(new Program());
            using var test = CStringArray.FromCommandLine();
            foreach (var item in test.Enumerate())
            {
                Console.WriteLine("CL item: " + item);
            }
            Sdl.Init(SdlInit.Video | SdlInit.Events).ThrowOnSdlFailure("Unable to init SDL.");
            {
                var program = new Program();
                program.OnStart();
                ApplicationManager.Run(program);
                program.OnStop();
            }
            Sdl.Quit();
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
            return 1;
        }
    }
}