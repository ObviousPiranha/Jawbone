namespace Jawbone.Sdl3;

public struct SdlHapticCondition // SDL_HapticCondition
{
    public ushort Type; // Uint16 type
    public nint Direction; // SDL_HapticDirection direction
    public uint Length; // Uint32 length
    public ushort Delay; // Uint16 delay
    public ushort Button; // Uint16 button
    public ushort Interval; // Uint16 interval
    public nint RightSat; // Uint16[3] right_sat
    public nint LeftSat; // Uint16[3] left_sat
    public nint RightCoeff; // Sint16[3] right_coeff
    public nint LeftCoeff; // Sint16[3] left_coeff
    public nint Deadband; // Uint16[3] deadband
    public nint Center; // Sint16[3] center
}
