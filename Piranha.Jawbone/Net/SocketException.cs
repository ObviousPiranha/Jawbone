using System;
using System.Runtime.Serialization;

namespace Piranha.Jawbone.Net;

[Serializable]
public class SocketException : Exception
{
    public SocketException() { }
    public SocketException(string message) : base(message) { }
    public SocketException(string message, Exception inner) : base(message, inner) { }
    protected SocketException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}