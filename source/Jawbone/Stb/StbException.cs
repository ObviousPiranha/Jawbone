using System;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone.Stb;

public sealed class StbException : Exception
{
    public StbException(
        string? message = null,
        Exception? innerException = null
        ) : base(message, innerException)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string? message)
    {
        throw new StbException(message);
    }
}
