namespace Piranha.Jawbone.Sdl3;

public struct SdlHapticCustom // SDL_HapticCustom
{
    public ushort Type; // Uint16 type
    public nint Direction; // SDL_HapticDirection direction
    public uint Length; // Uint32 length
    public ushort Delay; // Uint16 delay
    public ushort Button; // Uint16 button
    public ushort Interval; // Uint16 interval
    public byte Channels; // Uint8 channels
    public ushort Period; // Uint16 period
    public ushort Samples; // Uint16 samples
    public nint Data; // Uint16 * data
    public ushort AttackLength; // Uint16 attack_length
    public ushort AttackLevel; // Uint16 attack_level
    public ushort FadeLength; // Uint16 fade_length
    public ushort FadeLevel; // Uint16 fade_level
}
