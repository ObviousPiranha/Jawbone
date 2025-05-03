using System;

namespace Piranha.Jawbone.Net;

[Serializable]
public class SocketException : Exception
{
    public int Error => Code.Id;
    public ErrorCode Code { get; init; }

    public SocketException() { }
    public SocketException(string message) : base(message) { }
    public SocketException(string message, Exception inner) : base(message, inner) { }
}
