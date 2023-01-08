using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

public readonly ref struct SurfaceView
{
    public static readonly int FormatOffset = Environment.Is64BitProcess ? 8 : 4;
    public static readonly int WidthOffset = Environment.Is64BitProcess ? 16 : 8;
    public static readonly int HeightOffset = Environment.Is64BitProcess ? 20 : 12;
    public static readonly int PitchOffset = Environment.Is64BitProcess ? 24 : 16;
    public static readonly int PixelsOffset = Environment.Is64BitProcess ? 32 : 20;
    public static readonly int UserDataOffset = Environment.Is64BitProcess ? 40 : 24;

    public readonly IntPtr Address { get; }

    public readonly uint Flags
    {
        get => unchecked((uint)Marshal.ReadInt32(Address));
        set => Marshal.WriteInt32(Address, unchecked((int)value));
    }

    public readonly int Width
    {
        get => Marshal.ReadInt32(Address, WidthOffset);
        set => Marshal.WriteInt32(Address, WidthOffset, value);
    }

    public readonly int Height
    {
        get => Marshal.ReadInt32(Address, HeightOffset);
        set => Marshal.WriteInt32(Address, HeightOffset, value);
    }

    public readonly int Pitch
    {
        get => Marshal.ReadInt32(Address, PitchOffset);
        set => Marshal.WriteInt32(Address, PitchOffset, value);
    }

    public readonly IntPtr Pixels
    {
        get => Marshal.ReadIntPtr(Address, PixelsOffset);
        set => Marshal.WriteIntPtr(Address, PixelsOffset, value);
    }

    public readonly IntPtr UserData
    {
        get => Marshal.ReadIntPtr(Address, UserDataOffset);
        set => Marshal.WriteIntPtr(Address, UserDataOffset, value);
    }

    public readonly IntPtr Format
    {
        get => Marshal.ReadIntPtr(Address, FormatOffset);
        set => Marshal.WriteIntPtr(Address, FormatOffset, value);
    }

    public SurfaceView(IntPtr address)
    {
        Address = address;
    }

    public readonly Span<byte> GetPixelData()
    {
        unsafe
        {
            return new Span<byte>(
                Pixels.ToPointer(),
                Height * Pitch);
        }
    }
}
