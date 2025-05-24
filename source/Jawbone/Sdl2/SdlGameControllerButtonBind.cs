using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlGameControllerButtonBind
{
    public SdlControllerBindType BindType;
    public Union Value;

    [StructLayout(LayoutKind.Explicit)]
    public struct Union
    {
        [FieldOffset(0)] public int Button;
        [FieldOffset(0)] public int Axis;
        [FieldOffset(0)] public HatInfo Hat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HatInfo
    {
        public int Hat;
        public int HatMask;
    }
}
