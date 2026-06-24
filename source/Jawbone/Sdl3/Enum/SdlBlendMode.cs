using System;

namespace Jawbone.Sdl3;

public enum SdlBlendMode : uint
{
    None,
    Blend = 1 << 0,
    BlendPremultiplied = 1 << 4,
    Add = 1 << 1,
    AddPremultiplied = 1 << 5,
    Mod = 1 << 2,
    Mul = 1 << 3,
    Invalid = 0x7fffffff
}