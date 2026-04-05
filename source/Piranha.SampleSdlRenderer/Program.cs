using Jawbone;
using Jawbone.Sdl3;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Piranha.SampleSdlRenderer;

class Program : ISdlEventHandler, IDisposable
{
    private const int Width = 640;
    private const int Height = 480;

    private readonly nint _window;
    private readonly nint _renderer;
    private readonly nint _texture;
    private readonly Point32 _imageSize;

    private readonly long _frameLength;
    private long _nextFrame;
    private int _rotateTick;

    public bool Running { get; private set; } = true;

    public Program()
    {
        _frameLength = Stopwatch.Frequency / 60;
        _nextFrame = Stopwatch.GetTimestamp();
        Sdl.CreateWindowAndRenderer("Hello SDL3\0"u8[0], Width, Height, SdlWindowFlags.Resizable, out _window, out _renderer)
            .ThrowOnSdlFailure("Unable to create window and renderer.");
        Sdl.SetRenderLogicalPresentation(_renderer, Width, Height, SdlRendererLogicalPresentation.Letterbox)
            .ThrowOnSdlFailure("Error on SDL_SetRenderLogicalPresentation.");
        Console.WriteLine("So far so good.");
        var surface = Sdl.LoadPng("kenney_iconCross_blue.png\0"u8[0]).ThrowOnSdlFailure("Failed to load PNG.");
        _imageSize = SdlExtensions.GetSurfaceSize(surface);
        Console.WriteLine("Image size: " + _imageSize);
        _texture = Sdl.CreateTextureFromSurface(_renderer, surface).ThrowOnSdlFailure("Unable to create texture.");
        Sdl.DestroySurface(surface);

        {
            var version = SdlExtensions.GetVersion();
            Console.WriteLine(
                $"SDL version: {version.major}.{version.minor}.{version.micro}");
            var gpuDrivers = string.Join(", ", SdlExtensions.EnumerateGpuDrivers());
            Console.WriteLine($"Available GPU drivers: {gpuDrivers}");
            Console.WriteLine($"SDL video driver: {Sdl.GetCurrentVideoDriver()}");
            var gpuDevice = Sdl.GetGpuRendererDevice(_renderer);
            var gpuDeviceName = _renderer != default ? Sdl.GetGpuDeviceDriver(gpuDevice) : default;
            var finalName = gpuDeviceName.GetStringOrDefault("no GPU");
            Console.WriteLine("GPU device name: " + finalName);
        }
    }

    public void Dispose()
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
        var dstRect = new SdlFRect
        {
            X = (Width - _imageSize.X) / 2f,
            Y = (Height - _imageSize.Y) / 2f,
            W = _imageSize.X,
            H = _imageSize.Y
        };
        var center = new SdlFPoint
        {
            X = _imageSize.X / 2f,
            Y = _imageSize.Y / 2f
        };
        var angle = _rotateTick * 360 / 240d;
        Sdl.RenderTextureRotated(_renderer, _texture, Unsafe.NullRef<SdlFRect>(), dstRect, angle, center, SdlFlipMode.None);
        Sdl.RenderPresent(_renderer);
    }

    private static void Main(string[] args)
    {
        try
        {
            Sdl.Init(SdlInit.Video | SdlInit.Events).ThrowOnSdlFailure("Unable to init SDL.");
            {
                using var program = new Program();
                ApplicationManager.Run(program);
            }
            Sdl.Quit();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}