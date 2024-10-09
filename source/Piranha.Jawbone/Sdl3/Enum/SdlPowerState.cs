namespace Piranha.Jawbone.Sdl3;

public enum SdlPowerState // SDL_PowerState
{
    Error = -1, // SDL_POWERSTATE_ERROR
    Unknown = 0, // SDL_POWERSTATE_UNKNOWN
    OnBattery = 1, // SDL_POWERSTATE_ON_BATTERY
    NoBattery = 2, // SDL_POWERSTATE_NO_BATTERY
    Charging = 3, // SDL_POWERSTATE_CHARGING
    Charged = 4, // SDL_POWERSTATE_CHARGED
}
