using System;

namespace Piranha.Jawbone.Net;

public readonly struct SendResult
{
    private readonly int _value;

    public readonly SocketResult Result { get; }
    public readonly int BytesSent => Result == SocketResult.Success ? _value : 0;

    private SendResult(SocketResult result, int value = 0)
    {
        _value = value;
        Result = result;
    }

    public readonly void ThrowOnError()
    {
        if (Result == SocketResult.Error)
            ThrowError();
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

    public static SendResult Success(int length) => new(SocketResult.Success, length);
    public static SendResult Error(int error) => new(SocketResult.Error, error);
    public static SendResult Interrupt(int error = 0) => new(SocketResult.Interrupt, error);
}
