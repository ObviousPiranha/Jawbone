using System;

namespace Piranha.Jawbone.Sdl3;

public struct SdlBool
{
    public int Value;

    public override string ToString() => Convert.ToBoolean(Value).ToString();

    public static implicit operator bool(SdlBool sdlBool) => Convert.ToBoolean(sdlBool.Value);
    public static implicit operator SdlBool(bool b) => new SdlBool { Value = Convert.ToInt32(b) };
}
