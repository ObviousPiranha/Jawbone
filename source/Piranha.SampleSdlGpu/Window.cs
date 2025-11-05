using Jawbone.Sdl3;
using System;

namespace Jawbone.SampleSdlGpu;

struct Window : IDisposable
{
    public nint SdlWindow;
    public nint SdlDevice;
    public SdlFColor ClearColor;

    public Window(nint device)
    {
        SdlDevice = device;
        SdlWindow = Sdl.CreateWindow("Jawbone SDL GPU Sample", 1024, 768, SdlWindowFlags.Resizable)
            .ThrowOnSdlFailure("Unable to create window.");
        Sdl.ClaimWindowForGpuDevice(SdlDevice, SdlWindow).ThrowOnSdlFailure("Unable to claim window for GPU device.")
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