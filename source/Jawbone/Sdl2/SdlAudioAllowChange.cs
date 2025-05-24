using System;

namespace Jawbone.Sdl2;

[Flags]
public enum SdlAudioAllowChange
{
    None = 0,
    Frequency = 1 << 0,
    Format = 1 << 1,
    Channels = 1 << 2,
    Samples = 1 << 3,
    Any = Frequency | Format | Channels | Samples
}
