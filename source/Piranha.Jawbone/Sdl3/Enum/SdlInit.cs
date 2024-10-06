using System;

namespace Piranha.Jawbone.Sdl3;

[Flags]
public enum SdlInit : uint
{
    Audio = 0x00000010u,
    Video = 0x00000020u,
    Joystick = 0x00000200u,
    Haptic = 0x00001000u,
    Gamepad = 0x00002000u,
    Events = 0x00004000u,
    Sensor = 0x00008000u,
    Camera = 0x00010000u
}
