using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Sdl3;

public class SdlException : Exception
{
    public SdlException(string message) : base(message)
    {
    }

    public SdlException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public static SdlException Create(string? message = null)
    {
        message ??= "SDL error.";
        var sdlError = Sdl.GetError().ToString() ?? "Unknown SDL error";
        return new SdlException(message + " " + sdlError);
    }

    [DoesNotReturn]
    public static void Throw(string? message = null)
    {
        throw Create(message);
    }
}
