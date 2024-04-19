using System;

namespace Piranha.Jawbone.Sdl3;

[Flags]
public enum SdlInit : uint
{
    Timer = 1 << 0,
    Audio = 1 << 4,
    Video = 1 << 5,
    Joystick = 1 << 9,
    Haptic = 1 << 12,
    Gamepad = 1 << 13,
    Events = 1 << 14,
    Sensor = 1 << 15,
    Camera = 1 << 16
}
