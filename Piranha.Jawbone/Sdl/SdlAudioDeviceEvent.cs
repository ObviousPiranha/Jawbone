using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlAudioDeviceEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint Which;
    public byte IsCapture;
    public byte Padding1;
    public byte Padding2;
    public byte Padding3;
}
