namespace Piranha.Jawbone.Sdl3;

public struct SdlHapticPeriodic // SDL_HapticPeriodic
{
    public ushort Type; // Uint16 type
    public nint Direction; // SDL_HapticDirection direction
    public uint Length; // Uint32 length
    public ushort Delay; // Uint16 delay
    public ushort Button; // Uint16 button
    public ushort Interval; // Uint16 interval
    public ushort Period; // Uint16 period
    public short Magnitude; // Sint16 magnitude
    public short Offset; // Sint16 offset
    public ushort Phase; // Uint16 phase
    public ushort AttackLength; // Uint16 attack_length
    public ushort AttackLevel; // Uint16 attack_level
    public ushort FadeLength; // Uint16 fade_length
    public ushort FadeLevel; // Uint16 fade_level
}
