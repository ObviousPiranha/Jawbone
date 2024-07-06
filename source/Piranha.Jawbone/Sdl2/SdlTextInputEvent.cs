using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl2;

[StructLayout(LayoutKind.Sequential)]
public struct SdlTextInputEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public CharArray Text;

    // -- SDL_events.h --
    // #define SDL_TEXTINPUTEVENT_TEXT_SIZE (32)
    // char text[SDL_TEXTINPUTEVENT_TEXT_SIZE];
    [InlineArray(32)]
    public struct CharArray
    {
        private byte _a;
    }
}
