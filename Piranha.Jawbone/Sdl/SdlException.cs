using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Sdl;

public class SdlException : Exception
{
    public SdlException(string message) : base(message)
    {
    }

    public SdlException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [DoesNotReturn]
    public static void Throw(Sdl2Library sdl)
    {
        throw new SdlException(sdl.GetError().ToString() ?? "Unknown error");
    }
}
