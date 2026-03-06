using System;

namespace Jawbone.Sdl3;

public static class SdlPropGpuDevice
{
    public static class String
    {
        public static ReadOnlySpan<byte> GpuDeviceNameString => "SDL.gpu.device.name\0"u8;
    }
}
