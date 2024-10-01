using System;

namespace Piranha.Jawbone.Net;

[Serializable]
public class SocketException : Exception
{
    public static void ThrowOnError(int error, string message)
    {
        if (0 < error)
        {
            var exception = new SocketException(message)
            {
                Error = error
            };

            throw exception;
        }
    }

    public static ErrorCode GetErrorCode(int error)
    {
        if (OperatingSystem.IsWindows())
        {
            if (Windows.Sys.ErrorCodeById.TryGetValue(error, out var windowsErrorCode))
                return windowsErrorCode;
        }
        else
        {
            if (0 < error && error < Unix.Sys.ErrorCodes.Length)
                return Unix.Sys.ErrorCodes[error];
        }

        return ErrorCode.None;
    }

    public int Error { get; init; }
    public ErrorCode Code => GetErrorCode(Error);

    public SocketException() { }
    public SocketException(string message) : base(message) { }
    public SocketException(string message, Exception inner) : base(message, inner) { }
}
