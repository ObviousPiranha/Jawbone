using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone.Sdl2;

public class SdlException : Exception
{
    public SdlException(string message) : base(message)
    {
    }

    public SdlException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public static SdlException Create(Sdl2Library sdl)
    {
        return new SdlException(sdl.GetError().ToString() ?? "Unknown error");
    }

    [DoesNotReturn]
    public static void Throw(Sdl2Library sdl)
    {
        throw Create(sdl);
    }
}
