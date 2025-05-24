using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl3;

[InlineArray(Length)]
[StructLayout(LayoutKind.Sequential)]
public struct PenAxisArray
{
    public const int Length = 6;
    private float _a;
}
