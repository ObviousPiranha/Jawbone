using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

[StructLayout(LayoutKind.Sequential)]
public struct SdlTextEditingEvent
{
    public SdlEventType Type;
    public uint Timestamp;
    public uint WindowId;
    public CharArray Text;
    public int Start;
    public int Length;

    public static ReadOnlySpan<byte> GetText(
        in SdlTextEditingEvent sdlTextEditingEvent)
    {
        var bytes = MemoryMarshal.AsBytes(
            new ReadOnlySpan<CharArray>(
                in sdlTextEditingEvent.Text));
        return bytes;
    }

    // -- SDL_events.h --
    // #define SDL_TEXTEDITINGEVENT_TEXT_SIZE (32)
    // char text[SDL_TEXTEDITINGEVENT_TEXT_SIZE];
    [InlineArray(32)]
    public struct CharArray
    {
        private byte _a;
    }
}
