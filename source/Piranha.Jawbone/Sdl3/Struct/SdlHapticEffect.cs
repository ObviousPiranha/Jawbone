namespace Piranha.Jawbone.Sdl3;

public struct SdlHapticEffect // SDL_HapticEffect
{
    public ushort Type; // Uint16 type
    public nint Constant; // SDL_HapticConstant constant
    public nint Periodic; // SDL_HapticPeriodic periodic
    public nint Condition; // SDL_HapticCondition condition
    public nint Ramp; // SDL_HapticRamp ramp
    public nint Leftright; // SDL_HapticLeftRight leftright
    public nint Custom; // SDL_HapticCustom custom
}
