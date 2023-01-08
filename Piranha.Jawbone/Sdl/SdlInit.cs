namespace Piranha.Jawbone.Sdl;

public static class SdlInit
{
    public const uint Timer = 0x00000001u;
    public const uint Audio = 0x00000010u;
    public const uint Video = 0x00000020u;
    public const uint Joystick = 0x00000200u;
    public const uint Haptic = 0x00001000u;
    public const uint GameController = 0x00002000u;
    public const uint Events = 0x00004000u;
    public const uint Sensor = 0x00008000u;
    public const uint Everything = Timer | Audio | Video | Events | Joystick | Haptic | GameController | Sensor;
}
