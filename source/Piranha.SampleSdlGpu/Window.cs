using Jawbone.Sdl3;
using System;

namespace Jawbone.SampleSdlGpu;

readonly struct Window : IDisposable
{
    public readonly nint SdlWindow;
    public readonly nint SdlDevice;
    public readonly SdlFColor ClearColor;

    public Window(nint device)
    {
        SdlDevice = device;
        SdlWindow = Sdl.CreateWindow("Jawbone SDL GPU Sample", 1024, 768, SdlWindowFlags.Resizable)
            .ThrowOnSdlFailure("Unable to create window.");
        Sdl.ClaimWindowForGpuDevice(SdlDevice, SdlWindow)
            .ThrowOnSdlFailure("Unable to claim window for GPU device.");

        var r = Random.Shared;
        ClearColor = new SdlFColor
        {
            R = r.NextSingle(),
            G = r.NextSingle(),
            B = r.NextSingle(),
            A = 1f
        };
    }

    public void Dispose()
    {
        Sdl.ReleaseWindowFromGpuDevice(SdlDevice, SdlWindow);
        Sdl.DestroyWindow(SdlWindow);
    }
}