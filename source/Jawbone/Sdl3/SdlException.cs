using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone.Sdl3;

public sealed class SdlException : Exception
{
    public SdlException(
        string? message = null,
        Exception? innerException = null
        ) : base(message, innerException)
    {
    }

    [DoesNotReturn]
    public static void Throw(string? message = null, Exception? innerException = null)
    {
        message ??= "SDL error.";
        var sdlError = Sdl.GetError().ToString() ?? "Unknown SDL error";
        throw new SdlException((message + " " + sdlError).Trim(), innerException);
    }
}
