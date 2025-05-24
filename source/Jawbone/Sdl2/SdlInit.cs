namespace Jawbone.Sdl2;

public enum SdlInit : uint
{
    Timer = 1 << 0,
    Audio = 1 << 4,
    Video = 1 << 5,
    Joystick = 1 << 9,
    Haptic = 1 << 12,
    GameController = 1 << 13,
    Events = 1 << 14,
    Sensor = 1 << 15,
    Everything = Timer | Audio | Video | Events | Joystick | Haptic | GameController | Sensor
}
