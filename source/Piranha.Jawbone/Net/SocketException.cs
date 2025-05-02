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
        else if (OperatingSystem.IsMacOS())
        {
            if (0 < error && error < Mac.Error.Codes.Length)
                return Mac.Error.Codes[error];
        }
        else if (OperatingSystem.IsLinux())
        {
            if (0 < error && error < Linux.Error.Codes.Length)
                return Linux.Error.Codes[error];
        }
        else
        {
            throw new PlatformNotSupportedException();
        }

        return new(error, "UNKNOWN", "Unrecognized error code.");
    }

    public int Error { get; init; }
    public ErrorCode Code => GetErrorCode(Error);

    public SocketException() { }
    public SocketException(string message) : base(message) { }
    public SocketException(string message, Exception inner) : base(message, inner) { }
}
