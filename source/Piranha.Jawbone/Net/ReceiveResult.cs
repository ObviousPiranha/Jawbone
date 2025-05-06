using System;

namespace Piranha.Jawbone.Net;

public readonly struct ReceiveResult
{
    private readonly int _value;

    public readonly SocketResult Result { get; }
    public readonly int BytesReceived => Result == SocketResult.Success ? _value : 0;

    private ReceiveResult(SocketResult result, int value = 0)
    {
        _value = value;
        Result = result;
    }

    public readonly void ThrowOnError()
    {
        if (Result == SocketResult.Error)
            ThrowError();
    }

    public readonly void ThrowOnTimeout()
    {
        if (Result == SocketResult.Timeout)
            throw new TimeoutException();
    }

    public readonly void ThrowOnErrorOrTimeout()
    {
        ThrowOnError();
        ThrowOnTimeout();
    }

    private readonly void ThrowError()
    {
        var errorCode = SocketException.GetErrorCode(_value);
        var exception = new SocketException("Error on UDP receive: " + errorCode)
        {
            Code = errorCode
        };
        throw exception;
    }

    public static ReceiveResult Success(int length) => new(SocketResult.Success, length);
    public static ReceiveResult Error(int error) => new(SocketResult.Error, error);
    public static ReceiveResult Timeout() => new(SocketResult.Timeout);
    public static ReceiveResult Interrupt(int error = 0) => new(SocketResult.Interrupt, error);
}
