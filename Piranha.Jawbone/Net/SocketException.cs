using System;
using System.Runtime.Serialization;

namespace Piranha.Jawbone.Net;

[Serializable]
public class SocketException : Exception
{
    public static void ThrowOnError(int error, string message)
    {
        if (0 < error)
        {
            var errorCode = ErrorCode.None;

            if (OperatingSystem.IsWindows())
            {
                if (Windows.ErrorCodeById.TryGetValue(error, out var windowsErrorCode))
                    errorCode = windowsErrorCode;
            }
            else // Assume UNIX.
            {
                if (0 < error && error < Linux.ErrorCodes.Length)
                    errorCode = Linux.ErrorCodes[error];
            }

            var exception = new SocketException(message + " " + errorCode.ToString())
            {
                Code = errorCode
            };
            throw exception;
        }
    }

    public ErrorCode Code { get; private set; } = ErrorCode.None;

    public SocketException() { }
    public SocketException(string message) : base(message) { }
    public SocketException(string message, Exception inner) : base(message, inner) { }
    protected SocketException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}
