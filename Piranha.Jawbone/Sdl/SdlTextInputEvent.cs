using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlTextInputEvent
{
    public uint Type;
    public uint Timestamp;
    public uint WindowId;

    // -- SDL_events.h --
    // #define SDL_TEXTINPUTEVENT_TEXT_SIZE (32)
    // char text[SDL_TEXTINPUTEVENT_TEXT_SIZE];
    private uint _a;
    private uint _b;
    private uint _c;
    private uint _d;
    private uint _e;
    private uint _f;
    private uint _g;
    private uint _h;

    public static ReadOnlySpan<byte> GetText(in SdlTextInputEvent sdlTextInputEvent)
    {
        var bytes = MemoryMarshal.AsBytes(
            new ReadOnlySpan<SdlTextInputEvent>(sdlTextInputEvent));
        return bytes.Slice(12);
    }
}
