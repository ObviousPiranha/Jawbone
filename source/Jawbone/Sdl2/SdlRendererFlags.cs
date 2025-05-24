using System;

namespace Jawbone.Sdl2;

[Flags]
public enum SdlRendererFlags : uint
{
    None = 0,
    Software = 1 << 0,
    Accelerated = 1 << 1,
    PresentVSync = 1 << 2,
    TargetTexture = 1 << 3
}
