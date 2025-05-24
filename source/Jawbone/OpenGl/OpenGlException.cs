using System;
using System.Runtime.Serialization;

namespace Jawbone.OpenGl;

[Serializable]
public class OpenGlException : Exception
{
    public OpenGlException()
    {
    }

    public OpenGlException(string? message) : base(message)
    {
    }

    public OpenGlException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
